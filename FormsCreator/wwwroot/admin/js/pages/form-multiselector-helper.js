/**
 * 
 * @param {HTMLElement} inputElement
 */
function onTypeAfterError(inputElement) {
    const rootElement = inputElement.parentElement;
    inputElement.classList.remove('is-invalid');
    $(rootElement).find('.invalid-feedback').each((index, child) => {
        rootElement.removeChild(child);
    });
}

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

    // Limpiar los inputs ocultos existentes
    while (rootElement.firstChild) {
        rootElement.removeChild(rootElement.firstChild);
    }

    // Crear nuevos inputs ocultos basados en los valores seleccionados actuales
    currentValues.forEach((value, index) => {
        const hiddenInput = document.createElement('input');
        hiddenInput.type = 'hidden';
        hiddenInput.name = `${propertyName}[${index}].Id`;
        hiddenInput.value = value;
        hiddenInput.id = `${propertyName}[${index}].Id`;
        rootElement.appendChild(hiddenInput);
    });
}