function isNullOrWhiteSpace(str) {
    return typeof str === 'undefined' || str === null || str === '' || str.trim() === '';
}
  (function ($) {
  
  "use strict";

    // MENU
    $('.navbar-collapse a').on('click',function(){
      $(".navbar-collapse").collapse('hide');
    });
    
    // CUSTOM LINK
    $('.smoothscroll').click(function(){
      var el = $(this).attr('href');
      var elWrapped = $(el);
      var header_height = $('.navbar').height();
  
      scrollToDiv(elWrapped,header_height);
      return false;
  
      function scrollToDiv(element,navheight){
        var offset = element.offset();
        var offsetTop = offset.top;
        var totalScroll = offsetTop-0;
  
        $('body,html').animate({
        scrollTop: totalScroll
        }, 300);
      }
    });

    $('.owl-carousel').owlCarousel({
        center: true,
        loop: true,
        margin: 30,
        autoplay: true,
        responsiveClass: true,
        responsive:{
            0:{
                items: 2,
            },
            767:{
                items: 3,
            },
            1200:{
                items: 4,
            }
        }
    });
  
  })(window.jQuery);

// Función para resaltar el enlace activo en la barra de navegación
document.addEventListener('DOMContentLoaded', function () {
    // Obtener la URL actual
    const currentUrl = window.location.pathname;

    // Seleccionar todos los elementos <a> del navbar
    const navLinks = document.querySelectorAll('.navbar-nav .nav-link, .dropdown-item');

    // Recorrer los enlaces del navbar
    navLinks.forEach(function (link) {
        // Obtener el valor del href del enlace
        const linkHref = link.getAttribute('href');

        // Comprobar si el href coincide con la URL actual
        if (linkHref === currentUrl) {
            // Quitar la clase 'active' de todos los enlaces
            navLinks.forEach(function (link) {
                link.classList.remove('active');
            });

            // Añadir la clase 'active' al enlace coincidente
            link.classList.add('active');

            // Si es un elemento dentro de un dropdown, añadir la clase 'show' al dropdown padre
            var dropdownParent = link.closest('.dropdown-menu');
            if (dropdownParent) {
                var parentLink = dropdownParent.previousElementSibling;
                parentLink.classList.add('active');
            }
        }
    });
});

