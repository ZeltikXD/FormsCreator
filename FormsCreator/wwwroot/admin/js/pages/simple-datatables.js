/**Otorga estilo bootstrap 5 a las tablas de simple-datatables.js y pagina de manera personalizada en base a links de MVC.*/
class DataTableInit {
    #dataTable = null;
    #totalCount = 0;
    #currentPage = 1; // Nueva propiedad para la página actual

    /**Instancia del simple-dataTable actual*/
    get DataTable() {
        return this.#dataTable;
    }

    /**
     * Constructor para iniciar un nuevo dataTable.
     * @param {String} tableId - ID del elemento de la tabla
     * @param {Number} totalCount - Número total de elementos para paginación
     * @param {Number} currentPage - Página actual otorgado por el controlador.
     * @param {Number | undefined} initialCount - La cantidad inicial mostrada en la lista.
     */
    constructor(tableId, totalCount, currentPage, initialCount = undefined) {
        this.#totalCount = totalCount;
        this.#currentPage = currentPage;
        this.#dataTable = new simpleDatatables.DataTable(
            document.getElementById(tableId), {
            paging: true,
            perPage: typeof initialCount === 'undefined' ? this.#getPerPageOptions()[0] : initialCount, // Elige un valor por defecto
            perPageSelect: this.#getPerPageOptions()
        });

        // Patch "per page dropdown" and pagination after table rendered
        this.#dataTable.on("datatable.init", () => {
            this.#adaptPageDropdown();
            this.#refreshPagination();
        });

        this.#dataTable.on("datatable.update", this.#refreshPagination.bind(this));
        this.#dataTable.on("datatable.sort", this.#refreshPagination.bind(this));
        this.#dataTable.on("datatable.page", (event) => {
            this.#currentPage = event.detail.page + 1; // Actualizar la página actual
            this.#adaptPagination();
            this.#updateShowingEntriesText();
        });

        // Event listener for perPage change
        this.#dataTable.wrapper.querySelector(".dataTable-selector").addEventListener("change", (event) => {
            this.#refreshPageWithNewPageSize(event.target.value);
        });
    }

    /** Move "per page dropdown" selector element out of label to make it work with bootstrap 5. Add bs5 classes. */
    #adaptPageDropdown() {
        const selector = this.#dataTable.wrapper.querySelector(".dataTable-selector");
        selector.parentNode.parentNode.insertBefore(selector, selector.parentNode);
        selector.classList.add("form-select");
    }

    /** Add bs5 classes to pagination elements */
    #adaptPagination() {
        const paginations = this.#dataTable.wrapper.querySelectorAll("ul.dataTable-pagination-list");

        for (const pagination of paginations) {
            pagination.classList.add(...["pagination", "pagination-primary"]);
        }

        const paginationLis = this.#dataTable.wrapper.querySelectorAll("ul.dataTable-pagination-list li");

        for (const paginationLi of paginationLis) {
            paginationLi.classList.add("page-item");
        }

        const paginationLinks = this.#dataTable.wrapper.querySelectorAll("ul.dataTable-pagination-list li a");

        for (const paginationLink of paginationLinks) {
            paginationLink.classList.add("page-link");
        }
    }

    #refreshPagination() {
        const perPage = this.#dataTable.options.perPage;
        const totalPages = Math.ceil(this.#totalCount / perPage);
        this.customPagination(totalPages, perPage);
        this.#adaptPagination();
        this.#updateShowingEntriesText();
    }

    #getPerPageOptions() {
        return [5, 10, 15, 20, 25, 30];
    }

    #refreshPageWithNewPageSize(newPageSize) {
        const url = new URL(window.location.href);
        url.searchParams.set('page', 1);
        url.searchParams.set('size', newPageSize);
        window.location.href = url.toString();
    }

    #updateShowingEntriesText() {
        const perPage = this.#dataTable.options.perPage;
        const start = (this.#currentPage - 1) * perPage + 1;
        const end = Math.min(this.#currentPage * perPage, this.#totalCount);
        const text = `Showing ${start} to ${end} of ${this.#totalCount} entries`;

        /**@type {HTMLElement}*/
        const infoElement = this.#dataTable.wrapper.querySelector('.dataTable-info');
        if (infoElement) {
            infoElement.textContent = text;
        } else {
            const infoDiv = document.createElement('div');
            infoDiv.classList.add('dataTable-info');
            infoDiv.textContent = text;
            this.#dataTable.wrapper.appendChild(infoDiv);
        }
    }

    /**
     * Aplica los filtros y recarga la página con los parámetros de URL correspondientes
     * @param {object} filterParams
     */
    applyFilters(filterParams) {
        const url = new URL(window.location.href);
        url.searchParams.set('page', 1);
        for (const param in filterParams) {
            url.searchParams.set(param, filterParams[param]);
        }
        window.location.href = url.toString();
    }

    /**
     * Crea los enlaces de paginación personalizados
     */
    #createPaginationLinks(totalPages, pageSize) {
        let links = [];
        const maxVisiblePages = 4; // Máximo número de páginas visibles en la paginación

        // Botón "Anterior"
        if (this.#currentPage > 1) {
            let prevPageUrl = new URL(window.location.href);
            prevPageUrl.searchParams.set('page', this.#currentPage - 1);
            prevPageUrl.searchParams.set('size', pageSize);
            links.push(`<li class="page-item">
                            <a class="page-link" href="${prevPageUrl.toString()}" aria-label="Previous">
                                <span aria-hidden="true">&laquo;</span>
                            </a>
                        </li>`);
        }

        // Páginas compactadas
        if (totalPages <= maxVisiblePages) {
            for (let i = 1; i <= totalPages; i++) {
                let url = new URL(window.location.href);
                url.searchParams.set('page', i);
                url.searchParams.set('size', pageSize);
                const isCurrent = i === this.#currentPage;
                links.push(`<li class="page-item ${isCurrent ? 'active' : ''}">
                                <a class="page-link" href="${isCurrent ? 'javascript:void(0);' : url.toString()}">${i}</a>
                            </li>`);
            }
        } else {
            let startPage = Math.max(1, this.#currentPage - Math.floor(maxVisiblePages / 2));
            let endPage = Math.min(totalPages, this.#currentPage + Math.floor(maxVisiblePages / 2));

            if (startPage > 1) {
                let firstPageUrl = new URL(window.location.href);
                firstPageUrl.searchParams.set('page', 1);
                firstPageUrl.searchParams.set('size', pageSize);
                links.push(`<li class="page-item">
                                <a class="page-link" href="${firstPageUrl.toString()}">1</a>
                            </li>`);
                if (startPage > 2) {
                    links.push(`<li class="page-item disabled">
                                    <a class="page-link" href="#">...</a>
                                </li>`);
                }
            }

            for (let i = startPage; i <= endPage; i++) {
                let url = new URL(window.location.href);
                url.searchParams.set('page', i);
                url.searchParams.set('size', pageSize);
                const isCurrent = i === this.#currentPage;
                links.push(`<li class="page-item ${isCurrent ? 'active' : ''}">
                                <a class="page-link" href="${isCurrent ? 'javascript:void(0);' : url.toString()}">${i}</a>
                            </li>`);
            }

            if (endPage < totalPages) {
                if (endPage < totalPages - 1) {
                    links.push(`<li class="page-item disabled">
                                    <a class="page-link" href="#">...</a>
                                </li>`);
                }
                let lastPageUrl = new URL(window.location.href);
                lastPageUrl.searchParams.set('page', totalPages);
                lastPageUrl.searchParams.set('size', pageSize);
                links.push(`<li class="page-item">
                                <a class="page-link" href="${lastPageUrl.toString()}">${totalPages}</a>
                            </li>`);
            }
        }

        // Botón "Siguiente"
        if (this.#currentPage < totalPages) {
            let nextPageUrl = new URL(window.location.href);
            nextPageUrl.searchParams.set('page', this.#currentPage + 1);
            nextPageUrl.searchParams.set('size', pageSize);
            links.push(`<li class="page-item">
                            <a class="page-link" href="${nextPageUrl.toString()}" aria-label="Next">
                                <span aria-hidden="true">&raquo;</span>
                            </a>
                        </li>`);
        }

        return links.join('');
    }

    /**
     * Actualiza la paginación personalizada
     * @param {Number} totalPages 
     * @param {Number} pageSize 
     */
    customPagination(totalPages, pageSize) {
        const paginationWrapper = this.#dataTable.wrapper.querySelector("ul.dataTable-pagination-list");
        paginationWrapper.innerHTML = ''; // Limpiar la paginación existente
        paginationWrapper.innerHTML = this.#createPaginationLinks(totalPages, pageSize);
    }
}