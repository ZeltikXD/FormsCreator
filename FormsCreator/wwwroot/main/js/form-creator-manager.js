class TemplateManager {
    #questionCount = 0;
    #instanceName = '';
    #questionsList = document.getElementById('questions-list');
    #textQuestion = '';
    #descriptionText = '';
    #numberQuestion = '';
    #enterQuestion = '';
    #multipleChoiceQuestion = '';
    #checkboxQuestion = '';
    #dropdownQuestion = '';
    #multipleChoiceGrid = '';
    #checkboxGridQuestion = '';
    #optionText = '';
    #deleteText = '';
    #addOptionText = '';
    #columnText = '';
    #rowText = '';
    #addRowText = '';
    #addColumnText = '';

    #configurePropertys(opts) {
        this.#textQuestion = opts.TextQuestion;
        this.#descriptionText = opts.DescriptionText;
        this.#numberQuestion = opts.NumberQuestion;
        this.#enterQuestion = opts.EnterQuestionText;
        this.#multipleChoiceQuestion = opts.MultipleChoiceQuestion;
        this.#checkboxQuestion = opts.CheckboxQuestion;
        this.#dropdownQuestion = opts.DropdownQuestion;
        this.#multipleChoiceGrid = opts.MultipleChoiceGrid;
        this.#checkboxGridQuestion = opts.CheckboxGridQuestion;
        this.#optionText = opts.OptionText;
        this.#deleteText = opts.DeleteText;
        this.#addOptionText = opts.AddOptionText;
        this.#columnText = opts.ColumnText;
        this.#rowText = opts.RowText;
        this.#addRowText = opts.AddRowText;
        this.#addColumnText = opts.AddColumnText;
    }

    /**
     * The default constructor of TemplateManager.
     * @param {string} instanceName
     * @param {{}} opts
     */
    constructor(instanceName, opts, questionCount) {
        this.#instanceName = instanceName;
        this.#questionCount = questionCount;
        this.#configurePropertys(opts);
        // Initialize Sortable.js for drag & drop functionality
        new Sortable(this.#questionsList, {
            animation: 150,
            onEnd: () => {
                this.updateQuestionIndexes();
            }
        });
    }

    addQuestion(type) {
        const questionId = ++this.#questionCount;
        const questionHtml = this.#createQuestion(questionId, type);

        // Append the new question to the list
        this.#questionsList.insertAdjacentHTML('beforeend', questionHtml);

        // Update the indexes
        this.updateQuestionIndexes();
    }

    #createQuestion(id, type) {
        if (type === 2370 || type === 2371) {
            return `<li class="list-group-item" data-id="${id}">
                            <div class="mb-2">
                                ${this.getQuestionData(id, type)}
                            </div>
                            ${this.getQuestionFooter(id)}
                        </li>`;
        }
        else if (type === 2372 || type === 2374 || type === 2376) {
            return `<li class="list-group-item" data-id="${id}">
                            <div class="mb-2">
                                ${this.getQuestionData(id, type)}
                                <div class="${this.getClassFromType(type)}">
                                    <div class="input-group mb-2">
                                        <input type="text" name="Questions[${id - 1}].Options[0].Value" id="Questions[${id - 1}].Options[0].Value" class="form-control" value="${this.#optionText} 1" />
                                        <button type="button" class="btn btn-sm btn-danger" onclick="${this.#instanceName}.deleteOption(this, ${id})">${this.#deleteText}</button>
                                    </div>
                                    <div class="input-group mb-2">
                                        <input type="text" name="Questions[${id - 1}].Options[1].Value" id="Questions[${id - 1}].Options[1].Value" class="form-control" value="${this.#optionText} 2" />
                                        <button type="button" class="btn btn-sm btn-danger" onclick="${this.#instanceName}.deleteOption(this, ${id})">${this.#deleteText}</button>
                                    </div>
                                </div>
                                <button type="button" class="btn btn-sm btn-link" onclick="${this.#instanceName}.addOption('${id}', '${type}')">${this.#addOptionText}</button>
                            </div>
                            ${this.getQuestionFooter(id)}
                        </li>`;
        }
        else if (type === 2373 || type === 2375) {
            return `<li class="list-group-item" data-id="${id}">
            <div class="mb-2">
                ${this.getQuestionData(id, type)}
                <table class="table">
                    <thead>
                        <tr>
                            <th></th>
                            <th>
                                <div class="input-group">
                                    <input type="text" name="Questions[${id - 1}].Options[0].Column" id="Questions[${id - 1}].Options[0].Column" class="column-field form-control" value="${this.#columnText} 1" />
                                    <button type="button" class="btn btn-sm btn-danger" onclick="${this.#instanceName}.deleteGridColumn('${id}', 1)">${this.#deleteText}</button>
                                </div>
                            </th>
                            <th>
                                <div class="input-group">
                                    <input type="text" name="Questions[${id - 1}].Options[1].Column" id="Questions[${id - 1}].Options[1].Column" class="column-field form-control" value="${this.#columnText} 2" />
                                    <button type="button" class="btn btn-sm btn-danger" onclick="${this.#instanceName}.deleteGridColumn('${id}', 2)">${this.#deleteText}</button>
                                </div>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                <div class="input-group">
                                    <input type="text" name="Questions[${id - 1}].Options[0].Row" id="Questions[${id - 1}].Options[0].Row" class="row-field form-control" value="${this.#rowText} 1" />
                                    <button type="button" class="btn btn-sm btn-danger" onclick="${this.#instanceName}.deleteGridRow('${id}', 1)">${this.#deleteText}</button>
                                </div>
                            </td>
                            <td><input type="${this.getInputType(type)}" name="grid-${id}-row1" /></td>
                            <td><input type="${this.getInputType(type)}" name="grid-${id}-row1" /></td>
                        </tr>
                        <tr>
                            <td>
                                <div class="input-group">
                                    <input type="text" name="Questions[${id - 1}].Options[1].Row" id="Questions[${id - 1}].Options[1].Row" class="row-field form-control" value="${this.#rowText} 2" />
                                    <button type="button" class="btn btn-sm btn-danger" onclick="${this.#instanceName}.deleteGridRow('${id}', 2)">${this.#deleteText}</button>
                                </div>
                            </td>
                            <td><input type="${this.getInputType(type)}" name="grid-${id}-row2" /></td>
                            <td><input type="${this.getInputType(type)}" name="grid-${id}-row2" /></td>
                        </tr>
                    </tbody>
                </table>
                <button type="button" class="btn btn-sm btn-link" onclick="${this.#instanceName}.addGridRow('${id}', '${this.getInputType(type)}')">${this.#addRowText}</button>
                <button type="button" class="btn btn-sm btn-link" onclick="${this.#instanceName}.addGridColumn('${id}', '${this.getInputType(type)}')">${this.#addColumnText}</button>
            </div>
            ${this.getQuestionFooter(id)}
        </li>`; 
        }
    }

    #addDropdownOption(id) {
        const question = document.querySelector(`[data-id="${id}"] .dropdown-options`);
        const optionCount = question.querySelectorAll('.input-group').length; // Número de opciones actuales

        const newOptionHtml = `
    <div class="input-group mb-2">
        <input type="text" name="Questions[${id - 1}].Options[${optionCount}].Value" id="Questions[${id - 1}].Options[${optionCount}].Value" class="form-control" value="..." />
        <button type="button" class="btn btn-sm btn-danger" onclick="${this.#instanceName}.deleteDropdownOption(this)">${this.#deleteText}</button>
    </div>`;

        question.insertAdjacentHTML('beforeend', newOptionHtml);
    }

    #addOptionVanilla(id) {
        const question = document.querySelector(`[data-id="${id}"]`);
        const optionCount = question.querySelectorAll('.input-group').length; // Número de opciones actuales

        const newOptionHtml = `
    <div class="input-group mb-2">
        <input type="text" name="Questions[${id - 1}].Options[${optionCount}].Value" id="Questions[${id - 1}].Options[${optionCount}].Value" class="form-control" value="..." />
        <button type="button" class="btn btn-sm btn-danger" onclick="${this.#instanceName}.deleteOption(this, ${id})">${this.#deleteText}</button>
    </div>`;

        // Insertar la nueva opción antes del botón "Add Option"
        const button = question.querySelector('button[onclick*=addOption]');
        button.insertAdjacentHTML('beforebegin', newOptionHtml);
    }

    addOption(id, type) {
        if (type === 2376)
            return this.#addDropdownOption(id);

        return this.#addOptionVanilla(id);
    }

    addGridColumn(id, inputType) {
        const question = document.querySelector(`[data-id="${id}"]`);
        const table = question.querySelector('table');
        const theadRow = table.querySelector('thead tr');
        const tbodyRows = table.querySelectorAll('tbody tr');

        // Determinar el índice de la nueva columna
        const newColumnIndex = theadRow.children.length - 1; // Porque ya hay una columna extra de nombre

        // Agregar una nueva columna al encabezado
        const newHeaderCell = document.createElement('th');
        newHeaderCell.innerHTML = `<div class="input-group">
        <input type="text" name="Questions[${id - 1}].Options[${newColumnIndex}].Column" id="Questions[${id - 1}].Options[${newColumnIndex}].Column" class="column-field form-control" value="${this.#columnText} ${newColumnIndex + 1}" />
        <button type="button" class="btn btn-sm btn-danger" onclick="${this.#instanceName}.deleteGridColumn('${id}', ${newColumnIndex})">${this.#deleteText}</button>
    </div>`;
        theadRow.appendChild(newHeaderCell);

        // Agregar una nueva celda a cada fila del cuerpo
        tbodyRows.forEach((row, rowIndex) => {
            const newCell = document.createElement('td');
            newCell.innerHTML = `<input type="${inputType}" name="grid-${id}-row${rowIndex + 1}-col${newColumnIndex + 1}" />`;
            row.appendChild(newCell);
        });
    }

    addGridRow(id, inputType) {
        const question = document.querySelector(`[data-id="${id}"]`);
        const tbody = question.querySelector('tbody');
        const rowCount = tbody.rows.length + 1;
        const columnCount = tbody.rows[0].children.length - 1; // Restamos 1 por la columna de nombre de fila

        let newRowHtml = `<tr>
        <td><div class="input-group">
            <input type="text" name="Questions[${id - 1}].Options[${rowCount - 1}].Row" id="Questions[${id - 1}].Options[${rowCount - 1}].Row" class="row-field form-control" value="${this.#rowText} ${rowCount}" />
            <button type="button" class="btn btn-sm btn-danger" onclick="${this.#instanceName}.deleteGridRow('${id}', ${rowCount})">${this.#deleteText}</button>
        </div></td>`;

        for (let i = 1; i <= columnCount; i++) {
            newRowHtml += `<td><input type="${inputType}" name="grid-${id}-row${rowCount}-col${i}" /></td>`;
        }
        newRowHtml += `</tr>`;

        tbody.insertAdjacentHTML('beforeend', newRowHtml);
    }

    getQuestionData(id, type) {
        return `<label class="form-label">${this.getTextFromType(type)}</label>
                <input type="text" name="Questions[${id - 1}].Text" id="Questions[${id - 1}].Text" class="form-control" placeholder="${this.#enterQuestion}" />
                <label class="form-label">${this.#descriptionText}</label>
                <textarea class="form-control" id="Questions[${id - 1}].Description" name="Questions[${id - 1}].Description" rows="4"></textarea>
                <input type="hidden" name="Questions[${id - 1}].Index" id="Questions[${id - 1}].Index" class="question-index" value="${id - 1}" />
                <input type="hidden" name="Questions[${id-1}].Type" id="Questions[${id-1}].Type" value="${type}" /><br/>`;
    }

    getTextFromType(type) {
        switch (type) {
            case 2370:
                return this.#textQuestion;
            case 2371:
                return this.#numberQuestion;
            case 2372:
                return this.#multipleChoiceQuestion;
            case 2373:
                return this.#multipleChoiceGrid;
            case 2374:
                return this.#checkboxQuestion;
            case 2375:
                return this.#checkboxGridQuestion;
            case 2376:
                return this.#dropdownQuestion;
            default:
                return '';
        }
    }

    getClassFromType(type) {
        switch (type) {
            case 2372:
                return 'multiple-choice-options';
            case 2374:
                return 'checkbox-options';
            case 2376:
                return 'dropdown-options';
            default:
                return '';
        }
    }

    getInputType(type) {
        switch (type) {
            case 2372:
                return 'radio';
            case 2373:
                return 'radio';
            case 2374:
                return 'checkbox';
            case 2375:
                return 'checkbox';
            default:
                return '';
        }
    }

    getQuestionFooter(id) {
        return `<div class="d-flex justify-content-between align-items-center">
                        <button type="button" class="btn btn-sm btn-danger" onclick="${this.#instanceName}.deleteQuestion('${id}')">${this.#deleteText}</button>
                    </div>`;
    }

    updateQuestionIndexes() {
        const questions = document.querySelectorAll('#questions-list .list-group-item');

        questions.forEach((question, index) => {
            // Obtener el antiguo y nuevo ID
            const oldId = question.getAttribute('data-id');
            const newId = index + 1;
            question.querySelector('.question-index').value = newId;
            question.setAttribute('data-id', newId);

            // Actualizar todos los elementos que referencian el oldId en "name" y "id"
            const elementsToUpdate = question.querySelectorAll(`[name*="${oldId - 1}"], [id*="${oldId - 1}"], [for*="${oldId - 1}"]`);
            elementsToUpdate.forEach((element) => {
                if (element.name) {
                    element.name = element.name.replace(`Questions[${oldId - 1}]`, `Questions[${newId - 1}]`);
                }
                if (element.id) {
                    element.id = element.id.replace(`Questions[${oldId - 1}]`, `Questions[${newId - 1}]`);
                }
                if (element.htmlFor) {
                    element.htmlFor = element.htmlFor.replace(`Questions[${oldId - 1}]`, `Questions[${newId - 1}]`);
                }
            });

            // Actualizar los botones con eventos onclick que usen el id
            const buttonsToUpdate = question.querySelectorAll(`[onclick*="${oldId}"]`);
            buttonsToUpdate.forEach((button) => {
                const newOnclick = button.getAttribute('onclick').replace(oldId, newId);
                button.setAttribute('onclick', newOnclick);
            });
        });
    }


    deleteQuestion(id) {
        const question = document.querySelector(`[data-id="${id}"]`);
        question.remove();
        this.updateQuestionIndexes();
    }

    deleteOption(button, questionId) {
        const question = document.querySelector(`[data-id="${questionId}"]`);
        const optionsContainer = question.querySelector('.multiple-choice-options, .checkbox-options, .dropdown-options');

        // Eliminar la opción
        const option = button.closest('.input-group');
        option.remove();

        // Actualizar los índices de las opciones restantes
        this.updateOptionIndexes(questionId, optionsContainer, 'Options');
    }

    updateOptionIndexes(questionId, optionsContainer, optionType) {
        const options = optionsContainer.querySelectorAll('.input-group');

        options.forEach((option, index) => {
            // Actualizar el atributo name e id de cada input de opción
            const input = option.querySelector('input');
            input.name = `Questions[${questionId - 1}].${optionType}[${index}].Value`;
            input.id = `Questions[${questionId - 1}].${optionType}[${index}].Value`;
        });
    }

    deleteGridRow(questionId, rowIndex) {
        const question = document.querySelector(`[data-id="${questionId}"]`);
        const tbody = question.querySelector('tbody');

        // Eliminar la fila correspondiente
        const rowToDelete = tbody.querySelectorAll('tr')[rowIndex - 1];
        rowToDelete.remove();

        // Actualizar los índices de las filas restantes
        this.updateGridIndexes(questionId, tbody, 'Row');
    }

    deleteGridColumn(questionId, colIndex) {
        const question = document.querySelector(`[data-id="${questionId}"]`);
        const thead = question.querySelector('thead tr');
        const tbodyRows = question.querySelectorAll('tbody tr');

        // Eliminar la columna correspondiente en el thead
        thead.children[colIndex].remove();

        // Eliminar la columna correspondiente en cada fila del tbody
        tbodyRows.forEach(row => {
            row.children[colIndex].remove();
        });

        // Actualizar los índices de las columnas restantes
        this.updateGridIndexes(questionId, thead, 'Column');
    }

    updateGridIndexes(questionId, container, type) {
        const elements = container.querySelectorAll(`.${type.toLowerCase()}-field`);

        elements.forEach((element, index) => {
            // Actualizar el atributo name e id de cada input de fila/columna
            element.name = `Questions[${questionId - 1}].Options[${index}].${type}`;
            element.id = `Questions[${questionId - 1}].Options[${index}].${type}`;
        });
    }
}