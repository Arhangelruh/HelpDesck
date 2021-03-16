using HelpDesk.Common.Interfaces;
using System;

namespace HelpDesk.DAL.Models
{
    /// <summary>
    /// Event timer
    /// </summary>
    public class EventTime : IHasDbIdentity
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }
    }
}
