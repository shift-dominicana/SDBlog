﻿using SDBlog.Core.Base;

namespace SDBlog.DataModel.Entities
{
    public class PostTag : EntityBase
    {
        public int PostId { get; set; }
        public int TagId { get; set; }

        public Post Post { get; set; }
        public Tag Tag { get; set; }
    }
}
