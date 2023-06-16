using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scrap.UI.Models.Wordpress
{
    public class Post
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public DateTime date_gmt { get; set; }
        public Guid guid { get; set; }
        public DateTime modified { get; set; }
        public DateTime modified_gmt { get; set; }
        public string password { get; set; }
        public string slug { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string link { get; set; }
        public Title title { get; set; }
        public Content content { get; set; }
        public Excerpt excerpt { get; set; }
        public int author { get; set; }
        public int featured_media { get; set; }
        public string comment_status { get; set; }
        public string ping_status { get; set; }
        public bool sticky { get; set; }
        public string template { get; set; }
        public string format { get; set; }
        public Meta meta { get; set; }
        public List<int> categories { get; set; }
        public List<object> tags { get; set; }
        public string permalink_template { get; set; }
        public string generated_slug { get; set; }
        public List<object> aioseo_notices { get; set; }
        public Links _links { get; set; }
    }
    public class About
    {
        public string href { get; set; }
    }

    public class Author
    {
        public bool embeddable { get; set; }
        public string href { get; set; }
    }

    public class Collection
    {
        public string href { get; set; }
    }

    public class Content
    {
        public string raw { get; set; }
        public string rendered { get; set; }
        public bool @protected { get; set; }
        public int block_version { get; set; }
    }

    public class Cury
    {
        public string name { get; set; }
        public string href { get; set; }
        public bool templated { get; set; }
    }

    public class Excerpt
    {
        public string raw { get; set; }
        public string rendered { get; set; }
        public bool @protected { get; set; }
    }

    public class Guid
    {
        public string rendered { get; set; }
        public string raw { get; set; }
    }

    public class Links
    {
        public List<Self> self { get; set; }
        public List<Collection> collection { get; set; }
        public List<About> about { get; set; }
        public List<Author> author { get; set; }
        public List<Reply> replies { get; set; }

        [JsonProperty("version-history")]
        public List<VersionHistory> versionhistory { get; set; }

        [JsonProperty("wp:attachment")]
        public List<WpAttachment> wpattachment { get; set; }

        [JsonProperty("wp:term")]
        public List<WpTerm> wpterm { get; set; }

        [JsonProperty("wp:action-publish")]
        public List<WpActionPublish> wpactionpublish { get; set; }

        [JsonProperty("wp:action-unfiltered-html")]
        public List<WpActionUnfilteredHtml> wpactionunfilteredhtml { get; set; }

        [JsonProperty("wp:action-sticky")]
        public List<WpActionSticky> wpactionsticky { get; set; }

        [JsonProperty("wp:action-assign-author")]
        public List<WpActionAssignAuthor> wpactionassignauthor { get; set; }

        [JsonProperty("wp:action-create-categories")]
        public List<WpActionCreateCategory> wpactioncreatecategories { get; set; }

        [JsonProperty("wp:action-assign-categories")]
        public List<WpActionAssignCategory> wpactionassigncategories { get; set; }

        [JsonProperty("wp:action-create-tags")]
        public List<WpActionCreateTag> wpactioncreatetags { get; set; }

        [JsonProperty("wp:action-assign-tags")]
        public List<WpActionAssignTag> wpactionassigntags { get; set; }
        public List<Cury> curies { get; set; }
    }

    public class Meta
    {
        public bool om_disable_all_campaigns { get; set; }
        public bool inline_featured_image { get; set; }
        public bool _mi_skip_tracking { get; set; }
    }

    public class Reply
    {
        public bool embeddable { get; set; }
        public string href { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Title
    {
        public string raw { get; set; }
        public string rendered { get; set; }
    }

    public class VersionHistory
    {
        public int count { get; set; }
        public string href { get; set; }
    }

    public class WpActionAssignAuthor
    {
        public string href { get; set; }
    }

    public class WpActionAssignCategory
    {
        public string href { get; set; }
    }

    public class WpActionAssignTag
    {
        public string href { get; set; }
    }

    public class WpActionCreateCategory
    {
        public string href { get; set; }
    }

    public class WpActionCreateTag
    {
        public string href { get; set; }
    }

    public class WpActionPublish
    {
        public string href { get; set; }
    }

    public class WpActionSticky
    {
        public string href { get; set; }
    }

    public class WpActionUnfilteredHtml
    {
        public string href { get; set; }
    }

    public class WpAttachment
    {
        public string href { get; set; }
    }

    public class WpTerm
    {
        public string taxonomy { get; set; }
        public bool embeddable { get; set; }
        public string href { get; set; }
    }


}