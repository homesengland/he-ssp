using System.Text;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Radios
{
    /// <summary>
    /// Class GdsRadioInputLabelTagHelper.
    /// Implements the <see cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class GdsRadioInputLabelTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the label text.
        /// </summary>
        /// <value>The label text.</value>
        public string LabelText { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets for.
        /// </summary>
        /// <value>For.</value>
        public string For { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GdsRadioInputLabelTagHelper"/> is checked.
        /// </summary>
        /// <value><c>true</c> if checked; otherwise, <c>false</c>.</value>
        public bool Checked { get; set; }

        /// <summary>
        /// Synchronously executes the <see cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" /> with the given <paramref name="context" /> and
        /// <paramref name="output" />.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var sb = new StringBuilder(TagConstruct.ConstructRadioInputLabel(this.Id, this.Name, this.Value, this.LabelText, this.For));
            output?.Content.SetHtmlContent(sb.ToString());
        }
    }
}
