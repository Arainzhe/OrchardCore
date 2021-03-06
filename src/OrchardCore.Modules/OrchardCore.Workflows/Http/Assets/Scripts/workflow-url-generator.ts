///<reference path="../../../Assets/Lib/jQuery/typings.d.ts" />

$(() => {
    const generateWorkflowUrl = function () {
        const workflowDefinitionId: string = $('[data-workflow-definition-id]').data('workflow-definition-id');
        const activityId: string = $('[data-activity-id]').data('activity-id');
        const generateUrl: string = $('[data-generate-url]').data('generate-url') + `?workflowDefinitionId=${workflowDefinitionId}&activityId=${activityId}`;
        const antiforgeryHeaderName: string = $('[data-antiforgery-header-name]').data('antiforgery-header-name');
        const antiforgeryToken: string = $('[data-antiforgery-token]').data('antiforgery-token');
        const headers: any = {};

        headers[antiforgeryHeaderName] = antiforgeryToken;

        $.post({
            url: generateUrl,
            headers: headers
        }).done(url => {
            $('#workflow-url-text').val(url);
        });
    };

    $('#generate-url-button').on('click', e => {
        generateWorkflowUrl();
    });
});