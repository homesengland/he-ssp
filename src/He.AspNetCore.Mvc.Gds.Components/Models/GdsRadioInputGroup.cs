namespace He.AspNetCore.Mvc.Gds.Components.Models
{
    /// <summary>
    /// Class GdsRadioInputGroup.
    /// </summary>
    public class GdsRadioInputGroup
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
        /// Gets or sets a value indicating whether this <see cref="GdsRadioInputGroup"/> is checked.
        /// </summary>
        /// <value><c>true</c> if checked; otherwise, <c>false</c>.</value>
        public bool Checked { get; set; }

        /// <summary>
        /// Gets or sets the hint text.
        /// </summary>
        /// <value>The hint text.</value>
        public string HintText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GdsRadioInputGroup"/> is divider.
        /// </summary>
        /// <value><c>true</c> if divider; otherwise, <c>false</c>.</value>
        public bool Divider { get; set; }

        /// <summary>
        /// Gets or sets the divider text.
        /// </summary>
        /// <value>The divider text.</value>
        public string DividerText { get; set; }
    }
}
