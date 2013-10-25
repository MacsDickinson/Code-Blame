using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using CodeBlame.Models;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Common.Utils;
using Nancy;

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
                    var stream = _eventStoreConnection.ReadStreamEventsBackward("Messages", -1, int.MaxValue, true);

                    Model.Messages = new List<Message>(stream.Events.Count());
                    Model.Messages.AddRange(
                        stream.Events.Select(streamEvent => streamEvent.Event.Data.ParseJson<Message>()));
                    Model.Page.Title = "Code Blame - An Event Store & NancyFX Prototype";

                    return View["Index", Model];
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