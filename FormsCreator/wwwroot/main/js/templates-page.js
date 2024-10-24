$('#TopicsId').select2({
    theme: "bootstrap-5",
    width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
    placeholder: $(this).data('placeholder'),
    closeOnSelect: false,
});
$('#TagsId').select2({
    theme: "bootstrap-5",
    width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
    placeholder: $(this).data('placeholder'),
    closeOnSelect: false,
});

document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('topicsId').addEventListener('click', () => {
        const params = getValues();
        applyFilters({ topics: params.topics });
    });

    document.getElementById('tagsId').addEventListener('click', () => {
        const params = getValues();
        applyFilters({ tags: params.tags });
    });

    document.getElementById('text_search').addEventListener('click', () => {
        const params = getValues();
        applyFilters({ text: params.text });
    });

    function applyFilters(filterParams) {
        const url = new URL(window.location.href);
        const keys = [...url.searchParams.keys()];
        for (key of keys) {
            url.searchParams.delete(key);
        }
        url.searchParams.set('page', 1);
        for (const param in filterParams) {
            url.searchParams.set(param, filterParams[param]);
        }
        window.location.href = url.toString();
    }
});

function getValues() {
    return {
        tags: Array.from(document.getElementById('TagsId').selectedOptions).map(opt => opt.value).join('+'),
        topics: Array.from(document.getElementById('TopicsId').selectedOptions).map(opt => opt.value).join('+'),
        text: document.getElementById('Text_Search').value
    };
}