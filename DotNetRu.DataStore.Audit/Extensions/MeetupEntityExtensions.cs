﻿namespace DotNetRu.DataStore.Audit.Extensions
{
    using DotNetRu.DataStore.Audit.Entities;
    using DotNetRu.DataStore.Audit.Models;

    public static class MeetupEntityExtensions
    {
        public static MeetupModel ToModel(this MeetupEntity meetupEntity)
        {
            return new MeetupModel
                       {
                           CommunityID = meetupEntity.CommunityId,
                           Description = meetupEntity.Name,
                           IsAllDay = true,
                           Title = meetupEntity.Name,
                           StartTime = meetupEntity.Date,
                           EndTime = meetupEntity.Date,
                           VenueID = meetupEntity.VenueId,
                           TalkIDs = meetupEntity.TalkIds,
                           FriendIDs = meetupEntity.FriendIds
                       };
        }
    }
}
