window.addEventListener('scroll', () => {
    const toolbar = document.querySelector('.toolbar-id');
    const mainTemplate = document.getElementById('main-template');
    const footer = document.querySelector('footer');
    const toolbarHeight = toolbar.offsetHeight;
    const footerTop = footer.getBoundingClientRect().top + window.scrollY; // Obtener la posición superior del footer
    const scrollPosition = window.scrollY + toolbarHeight;
    const stickyTop = 160; // Ajusta el valor para que coincida con la altura del navbar

    // Si la barra ha llegado al footer
    if (scrollPosition >= (footerTop - 15)) {
        toolbar.classList.remove('sidenav');
        toolbar.style.position = 'absolute';
        toolbar.style.top = `${footerTop - toolbarHeight}px`; // Fijarla justo antes del footer
    }
    // Si la barra debería estar sticky en su posición fija
    else if (window.scrollY >= stickyTop) {
        toolbar.style.position = '';
        toolbar.style.top = ``; // Ajuste para el navbar
        toolbar.classList.add('sidenav');
        mainTemplate.classList.add('main-template');
    }
    // Cuando el usuario está por encima del stickyTop, volver a la posición relativa normal
    else {
        toolbar.classList.remove('sidenav');
        mainTemplate.classList.remove('main-template');
        toolbar.style.position = 'relative';
        toolbar.style.top = '0';
    }
});