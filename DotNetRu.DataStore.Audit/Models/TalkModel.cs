﻿namespace DotNetRu.DataStore.Audit.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Xamarin.Forms;

    public class TalkModel : BaseModel
    {
        private const string Delimiter = "|";

        private bool feedbackLeft;
        private string haystack;

        public TalkModel()
        {
            this.Speakers = new List<SpeakerModel>();
            this.Categories = new List<Category>();
        }

        public string TalkId { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the short title that is displayed in the navigation bar
        /// For instance "Intro to X.Forms"
        /// </summary>
        /// <value>The short title.</value>
        public string ShortTitle { get; set; }

        public string Abstract { get; set; }

        public ICollection<SpeakerModel> Speakers { get; set; }

        public Room Room { get; set; }

        public ICollection<Category> Categories { get; set; }

        public DateTime? StartTime => this.MeetupModel.StartTime;

        public DateTime? EndTime => this.MeetupModel.EndTime;

        /// <summary>
        /// Gets or sets the level of the session [100 - 400]
        /// </summary>
        /// <value>The session level.</value>
        public string Level { get; set; }

        public string PresentationUrl { get; set; }

        public string VideoUrl { get; set; }

        public string CodeUrl { get; set; }

        public string Haystack
        {
            get
            {
                if (this.haystack != null)
                {
                    return this.haystack;
                }

                var builder = new StringBuilder();
                builder.Append(Delimiter);
                builder.Append(this.Title);
                builder.Append(Delimiter);
                if (this.Categories != null)
                {
                    foreach (var c in this.Categories)
                    {
                        builder.Append($"{c.Name}{Delimiter}{c.ShortName}{Delimiter}");
                    }
                }

                if (this.Speakers != null)
                {
                    foreach (var p in this.Speakers)
                    {
                        builder.Append(
                            $"{p.FirstName} {p.LastName}{Delimiter}{p.FirstName}{Delimiter}{p.LastName}{Delimiter}");
                    }
                }

                this.haystack = builder.ToString();
                return this.haystack;
            }
        }

        public ImageSource SpeakerAvatar => this.Speakers.Count > 1
                                                ? ImageSource.FromResource(
                                                    "DotNetRu.DataStore.Audit.Storage.SeveralSpeakers.png")
                                                : this.Speakers.First().AvatarImage;

        public string SpeakerNames => string.Join(",", this.Speakers.Select(x => x.FullName));

        public ImageSource CommunityLogo => ImageSource.FromResource(
            "DotNetRu.DataStore.Audit.Storage.logos." + this.MeetupModel.CommunityID + ".png");

        public bool FeedbackLeft
        {
            get => this.feedbackLeft;

            set => this.SetProperty(ref this.feedbackLeft, value);
        }

        internal MeetupModel MeetupModel { get; set; }
    }
}