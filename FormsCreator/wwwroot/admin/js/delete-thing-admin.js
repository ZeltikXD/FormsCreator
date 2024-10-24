/**
 * Función encargada de enviar solicitudes de eliminación. Requiere de SweetAlert2.
 * @param {string} url Dirección de eliminación del objeto. Todos deben tener una devolución estándar.
 * @param {string} description Descripción que aparecerá en el mensaje para confirmar la eliminación.
 */
function sendDeleteRequest(url, description) {
    const swalMix = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-danger add-margin',
            cancelButton: 'btn btn-primary add-margin'
        },
        buttonsStyling: false
    });

    swalMix.fire({
        title: '&iquest;Est&aacute; seguro de eliminar este elemento?',
        text: description,
        icon: 'warning',
        showLoaderOnConfirm: true,
        showCancelButton: true,
        confirmButtonText: '&iexcl;S&iacute;, b&oacute;rralo!',
        cancelButtonText: 'Cancelar',
        preConfirm: deleteAsync
    }).then((result) => {
        if (result.isConfirmed && result.value.success) {
            Swal.fire({
                title: 'Elemento eliminado correctamente',
                text: result.value.mensaje,
                icon: 'success',
                confirmButtonText: 'Entendido'
            }).then(() => location.reload());
        }
    });

    async function deleteAsync() {
        try {
            const res = await fetch(url, { method: 'DELETE' });
            const { code, message } = await res.json();
            if (!res.ok) {
                const codigo = typeof code === 'undefined' ? 'unknown' : code;
                showErrorMessage('API Message: ' + message + ' | Codigo interno: ' + codigo);
                return { success: false, mensaje: null };
            }
            return { mensaje: message, success: true };
        } catch (error) {
            showErrorMessage(error.message);
            console.error(error);
            return { success: false, mensaje: null };
        }

        function showErrorMessage(text) {
            Swal.fire({
                title: '&iexcl;Ups! Algo ocurri&oacute; durante la operaci&oacute;n',
                text: text,
                icon: 'error',
                confirmButtonText: 'Entendido'
            }).then(() => location.reload());
        }
    }
}