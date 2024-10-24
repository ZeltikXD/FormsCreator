document.addEventListener('DOMContentLoaded', function () {
    const choices = document.querySelectorAll(".choices")
    const initChoices = [];
    choices.forEach((item, i) => {
        if (item.classList.contains("multiple-remove")) {
            initChoices[i] = new Choices(item, {
                delimiter: ",",
                editItems: true,
                maxItemCount: -1,
                removeItemButton: true,
                allowHTML: true
            });
            setUpInvalidInput(item.classList.contains('is-invalid'), item, initChoices[i].containerOuter.element);
        } else {
            initChoices[i] = new Choices(item)
        }
    });

    /**
     * @param {boolean} isInvalid
     * @param {HTMLSelectElement} elementToListen 
     * @param {HTMLElement} elementToApply
     */
    function setUpInvalidInput(isInvalid, elementToListen, elementToApply) {
        if (isInvalid) {
            elementToApply.classList.add('is-invalid');

            elementToListen.addEventListener('change', () => {
                elementToApply.classList.remove('is-invalid');
            });
        }
    }
});