/**
 * @typedef {Object} User
 * @property {string} userName
 * @property {string} id
 * @property {string} createdAt
 */

/**
 * @typedef {Object} TemplComment
 * @property {string} content
 * @property {User} user
 * @property {string} id
 * @property {string} templateId
 * @property {string} createdAt
 */

class HandleComments {
    #apiUrl = '';
    #templateId = '';
    #currentPage = 0;
    #currentLocale = 'en-US';
    #defaultMessage = '';

    get CurrentPage() {
        return this.#currentPage;
    }

    get ApiUrl() {
        return this.#apiUrl;
    }

    get DefElText() {
        return '<h5>{text}</h5>';
    }

    get DefaultElement() {
        const node = document.createElement('div');
        node.classList.add('__token');
        node.innerHTML = this.DefElText.replace('{text}', this.#defaultMessage);
        return node;
    }

    constructor(apiUrl, templateId, locale) {
        this.#apiUrl = apiUrl;
        this.#templateId = templateId;
        this.#currentLocale = locale === 'es' ? 'es-MX' : 'en-US';
        this.#defaultMessage = locale === 'es' ? 'No hay comentarios, ¡Se el primero!' : 'There are no comments, ¡Be the first!';
    }

    /**
     * 
     * @param {string} dataContainerId
     * @param {string} paginationContainerId 
     * @param {number} totalComments
     * @param {number} pageSize 
     */
    paginate(dataContainerId, paginationContainerId, totalComments, pageSize) {
        const dataContainer = $(`#${dataContainerId}`);
        const paginationContainer = document.getElementById(paginationContainerId);
        $(paginationContainer).pagination({
            dataSource: this.#apiUrl,
            locator: '',
            totalNumber: totalComments,
            pageSize: pageSize,
            ajax: {
                beforeSend: () => {
                    const defaultElement = this.#customDefaultElement(this.#currentLocale === 'en-US' ? 'Loading comments...' : 'Cargando comentarios...');
                    dataContainer.html(defaultElement);
                }
            },
            alias: {
                pageNumber: 'page',
                pageSize: 'size'
            },
            className: 'paginationjs-theme-blue',
            callback: (items, pagination) => {
                const html = items.length > 0 ? this.#template(items)
                    : HandleComments.DefaultElement;
                this.#currentPage = pagination.pageNumber;
                dataContainer.html(html);
                this.#setStyles(paginationContainer.querySelector('ul'));
            }
        });
    }

    /**
     * 
     * @param {string} dataContainerId
     * @param {string} paginationContainerId 
     * @param {number} totalComments
     * @param {number} pageSize 
     * @param {number} currentPage 
     */
    paginateWithPage(dataContainerId, paginationContainerId, totalComments, pageSize, currentPage) {
        const dataContainer = $(`#${dataContainerId}`);
        const paginationContainer = document.getElementById(paginationContainerId);
        $(paginationContainer).pagination({
            dataSource: this.#apiUrl,
            locator: '',
            totalNumber: totalComments,
            pageNumber: currentPage,
            pageSize: pageSize,
            ajax: {
                beforeSend: () => {
                    const defaultElement = this.#customDefaultElement(this.#currentLocale === 'en-US' ? 'Loading comments...' : 'Cargando comentarios...');
                    dataContainer.html(defaultElement);
                }
            },
            alias: {
                pageNumber: 'page',
                pageSize: 'size'
            },
            className: 'paginationjs-theme-blue',
            callback: (items, pagination) => {
                const html = items.length > 0 ? this.#template(items)
                    : HandleComments.DefaultElement;
                this.#currentPage = pagination.pageNumber;
                dataContainer.html(html);
                this.#setStyles(paginationContainer.querySelector('ul'));
            }
        });
    }

    /**
     * @param {HTMLUListElement} ulElement
     */
    #setStyles(ulElement) {
        ulElement.classList.add('pagination');
        ulElement.classList.add('pagination-sm');
        ulElement.classList.add('mt-5');

        ulElement.querySelectorAll('li').forEach((li) => {
            li.classList.add('page-item');
            li.querySelector('a').classList.add('page-link');
        });
    }

    /**
     * 
     * @param {TemplComment[]} items
     */
    #template(items) {
        /**@type {HTMLTemplateElement}*/
        const template = document.getElementById(this.#templateId);

        const getDate = (dateString) => {
            const epoch = Date.parse(dateString);
            const date = new Date(epoch);
            return date.toLocaleString(this.#currentLocale);
        }

        return items.map(comment => {
            const clone = template.content.cloneNode(true);

            // Replace userName
            /**@type {HTMLElement}*/
            const userNameElement = clone.querySelector('h6');
            userNameElement.textContent = comment.user.userName;

            // Replace timeStamp
            /**@type {HTMLParagraphElement}*/
            const timeStampElement = clone.querySelector('p.text-muted.small.mb-0');
            timeStampElement.textContent = ` ${getDate(comment.createdAt)}`;

            // Replace text
            /**@type {HTMLParagraphElement}*/
            const textElement = clone.querySelector('p.mt-3.mb-4.pb-2');
            textElement.textContent = comment.content;

            return clone;
        });
    }

    /**
     * @param {string} text
     */
    #customDefaultElement(text) {
        const element = this.DefaultElement;
        element.innerHTML = this.DefElText.replace('{text}', text);
        return element;
    }
}