document.getElementById('submit-jira-form').addEventListener('click', (ev) => {
    ev.target.disabled = true;
    const summary = document.getElementById('jira-summary').value;
    const priority = document.getElementById('jira-priority').value;
    const templateTitle = document.getElementById('jira-template-title').value;
    const formData = new FormData();
    formData.append('Summary', summary);
    formData.append('Priority', priority);
    formData.append('TemplateTitle', templateTitle);
    sendRequest({ url: '/api/v1/atlassian/jira/create-ticket', method: 'POST', body: formData })
        .then(() => { ev.target.disabled = false });

    async function sendRequest({ url, method, body }) {
        const res = await fetch(url, { method: method, body: body });
        if (res.redirected) {
            location.href = res.url;
            return;
        }
        alert(JSON.stringify(await res.json()));
    }
});