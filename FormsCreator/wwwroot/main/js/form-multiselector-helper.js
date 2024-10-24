/**
 * 
 * @param {String} selectorId
 * @param {String} containerId
 * @param {String} propertyName
 */
function adjustMultipleSelector(selectorId, containerId, propertyName) {
    const rootElement = document.getElementById(containerId);
    /** @type {HTMLSelectElement} */
    const selectorElement = document.getElementById(selectorId);
    const currentValues = Array.from(selectorElement.selectedOptions).map(opt => opt.value);

    while (rootElement.firstChild) {
        rootElement.removeChild(rootElement.firstChild);
    }

    currentValues.forEach((value, index) => {
        const hiddenInput = document.createElement('input');
        hiddenInput.type = 'hidden';
        hiddenInput.name = `${propertyName}[${index}].Id`;
        hiddenInput.value = value;
        hiddenInput.id = `${propertyName}[${index}].Id`;
        rootElement.appendChild(hiddenInput);
    });
}

function adjustMultipleSelectorWithNames(selectorId, containerId, propertyName, subPropertyName) {
    const rootElement = document.getElementById(containerId);
    /** @type {HTMLSelectElement} */
    const selectorElement = document.getElementById(selectorId);
    const currentValues = Array.from(selectorElement.selectedOptions).map(opt => opt);

    while (rootElement.firstChild) {
        rootElement.removeChild(rootElement.firstChild);
    }

    currentValues.forEach((opt, index) => {
        const hiddenInput = document.createElement('input');
        hiddenInput.type = 'hidden';
        hiddenInput.name = `${propertyName}[${index}].Id`;
        hiddenInput.value = opt.value;
        hiddenInput.id = `${propertyName}[${index}].Id`;

        const hiddenInput2 = document.createElement('input');
        hiddenInput2.type = 'hidden';
        hiddenInput2.name = `${propertyName}[${index}].${subPropertyName}`;
        hiddenInput2.id = `${propertyName}[${index}].${subPropertyName}`;
        hiddenInput2.value = opt.text;
        rootElement.appendChild(hiddenInput);
        rootElement.appendChild(hiddenInput2);
    });
}