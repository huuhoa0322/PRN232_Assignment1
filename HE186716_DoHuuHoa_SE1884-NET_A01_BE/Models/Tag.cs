using System;
using System.Collections.Generic;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_BE.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public string? TagName { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
