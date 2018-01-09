using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Helpers;
using OrchardCore.Workflows.Models;

namespace OrchardCore.Contents.Workflows.Activities
{
    public abstract class ContentEvent : EventActivity
    {
        protected ContentEvent(IContentManager contentManager, IStringLocalizer s)
        {
            ContentManager = contentManager;
            S = s;
        }

        protected IContentManager ContentManager { get; }
        protected IStringLocalizer S { get; }
        public override LocalizedString Category => S["Content"];

        public IList<string> ContentTypeFilter
        {
            get => GetProperty<IList<string>>(defaultValue: () => new List<string>());
            set => SetProperty(value);
        }

        /// <summary>
        /// An expression that evaluates to either an <see cref="IContent"/> item or a content item ID.
        /// </summary>
        public WorkflowExpression<object> ContentExpression
        {
            get => GetProperty(() => new WorkflowExpression<object>());
            set => SetProperty(value);
        }

        public override async Task<bool> CanExecuteAsync(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            var content = await GetContentAsync(workflowContext);

            if (content == null)
            {
                return false;
            }

            var contentTypes = ContentTypeFilter.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            // "" means 'any'.
            return !contentTypes.Any() || contentTypes.Any(contentType => content.ContentItem.ContentType == contentType);
        }

        public override IEnumerable<Outcome> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            return Outcomes(S["Done"]);
        }

        public override IEnumerable<string> Execute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            yield return "Done";
        }

        protected async Task<IContent> GetContentAsync(WorkflowContext workflowContext)
        {
            // Try and evaluate a content item from the ContentExpression, if provided.
            // If no expression was provided, assume the content item was provided as an input using the "Content" key.
            var contentValue = !String.IsNullOrWhiteSpace(ContentExpression.Expression)
                ? await workflowContext.EvaluateAsync(ContentExpression)
                : workflowContext.Input.GetValue("Content");

            var contentId = contentValue as string;

            if (contentId != null)
            {
                return await ContentManager.GetAsync(contentId, VersionOptions.Latest);
            }

            var content = contentValue as IContent;
            return content;
        }
    }
}