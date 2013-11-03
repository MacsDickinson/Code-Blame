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

        private readonly IEventStoreConnection _eventStoreConnection;

        public BaseModule(IEventStoreConnection connection)
        {
            _eventStoreConnection = connection;

            Get["/"] = ಠ_ಠ =>
                {
                    Model.Title = "Code Blame - An Event Store & NancyFX Prototype";
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

            Get["/GetBlames"] = ಠ_ಠ =>
                {
                    var stream = _eventStoreConnection.ReadStreamEventsBackward("Blames", -1, int.MaxValue, true);
                    return stream.Events.Select(streamEvent => streamEvent.Event.Data.ParseJson<Blame>());
                };

            Post["/Add"] = ಠ_ಠ =>
                {
                    var model = this.Bind<Blame>();
                    model.DateAdded = DateTime.Now;

                    var eventData = new List<EventData>
                        {
                            new EventData(Guid.NewGuid(), "Blame", true, model.ToJsonBytes(), null)
                        };
                    _eventStoreConnection.AppendToStream("Blames", ExpectedVersion.Any, eventData);
                    return null;
                };
        }
    }
}