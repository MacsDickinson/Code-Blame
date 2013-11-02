using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using CodeBlame.Models;
using CodeBlame.Models.Enums;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Common.Utils;
using Nancy;
using Nancy.ModelBinding;

namespace CodeBlame.Modules
{
    public class BaseModule : NancyModule
    {
        public dynamic Model = new ExpandoObject();

        protected PageModel Page { get; set; }

        private readonly IEventStoreConnection _eventStoreConnection;

        public BaseModule(IEventStoreConnection connection)
        {
            _eventStoreConnection = connection;

            SetupModelDefaults();

            Get["/"] = ಠ_ಠ =>
                {
                    var stream = _eventStoreConnection.ReadStreamEventsBackward("Blames", -1, int.MaxValue, true);

                    Model.Messages = new List<Blame>(stream.Events.Count());
                    Model.Messages.AddRange(stream.Events.Select(streamEvent => streamEvent.Event.Data.ParseJson<Blame>()));
                    Model.Page.Title = "Code Blame - An Event Store & NancyFX Prototype";

                    Model.Languages = new Dictionary<int, string>();

                    var type = typeof(BlameLanguage);
                    foreach (var value in Enum.GetValues(typeof(BlameLanguage)))
                    {
                        var member = type.GetMember(value.ToString());
                        var attributes = member[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                        Model.Languages.Add((int)value, ((DisplayAttribute)attributes[0]).Name);
                    }
                    return View["Index", Model];
                };

            Post["/Add"] = ಠ_ಠ =>
                {
                    var model = this.Bind<Blame>();
                    model.DateAdded = DateTime.Now;

                    var eventData = new List<EventData>();
                    eventData.Add(new EventData(Guid.NewGuid(), "Blame", true, model.ToJsonBytes(), null));
                    _eventStoreConnection.AppendToStream("Blames", ExpectedVersion.Any, eventData);
                    return null;
                };

            Get["/CreateDummy"] = ಠ_ಠ =>
                {
                    var message = new Message
                        {
                            Content = "Dummy Message",
                            Sent = DateTime.Now,
                            To = "Macs"
                        };
                    var eventData = new List<EventData>();

                    var json = message.ToJsonBytes();
                    eventData.Add(new EventData(Guid.NewGuid(), "Message", true, json, null));

                    _eventStoreConnection.AppendToStream("Messages", ExpectedVersion.Any, eventData);

                    return "Added";
                };
        }

        private void SetupModelDefaults()
        {
            Before += ctx =>
            {
                Page = new PageModel
                {
                    IsAuthenticated = ctx.CurrentUser != null,
                    TitleSuffix = " | Just For Fun",
                    Notifications = new List<NotificationModel>()
                };

                Model.Page = Page;

                return null;
            };
        }
    }
}