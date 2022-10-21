using HelpDesk.Web.Services;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace HelpDesk.Web.TagHelpers
{
    public class InstructionTagHelper : TagHelper
    {
        /// <summary>
        /// reference to link.
        /// </summary>
        private string _hrefTagFlag = "[Href|";

        /// <summary>
        /// flag for devide text.
        /// </summary>
        private string _hrefTagDivide = "||";

        /// <summary>
        /// flag ending text.
        /// </summary>
        private string _hrefEnd = "]";

        /// <summary>
        /// Folder for instructions 'wwwroot/instructions/'
        /// </summary>
        private string _instructionFolder = "/instructions/";

        /// <summary>
        /// Get reference from text, reference notice format:[Href|text reference||reference].
        /// </summary>
        [HtmlAttributeName("tagtext")]
        public string InsructionText { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "p";
            StringBuilder stringBuilder = new StringBuilder();

            if (InsructionText.Contains(_hrefTagFlag))
            {
                var strings = InsructionText.FindAllBlocks(_hrefTagFlag, _hrefEnd);

                foreach (var block in strings)
                {
                    if (block.Contains(_hrefTagFlag))
                    {
                        var hrefText = block.Between(_hrefTagFlag, _hrefTagDivide);
                        var href = block.Between(_hrefTagDivide, _hrefEnd);
                        stringBuilder.Append(@"<a href=""" + _instructionFolder + href + "\"" + ">" + hrefText + "</a>");
                    }
                    else
                    {
                        stringBuilder.Append(block.Replace("\r\n", "<br/>"));
                    }
                }
            }
            else
            {
                stringBuilder.Append(InsructionText.Replace("\r\n", "<br/>"));
            }
            output.Content.SetHtmlContent(stringBuilder.ToString());
        }
    }


}
