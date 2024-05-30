// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    const searchIcon = document.getElementById("search-icon");
    const searchForm = document.getElementById("search-form");

    searchIcon.addEventListener("click", function (event) {
        event.preventDefault();
        if (searchForm.classList.contains("show")) {
            searchForm.style.opacity = '0'; 
            setTimeout(() => {
                searchForm.classList.remove("show");
                searchForm.style.height = '0'; 
            }, 300); 
        } else {
            searchForm.style.opacity = '1';
            searchForm.classList.add("show");
            searchForm.style.height = 'auto'; 
        }
    });

    document.addEventListener("click", function (event) {
        if (!searchIcon.contains(event.target) && !searchForm.contains(event.target)) {
            if (searchForm.classList.contains("show")) {
                searchForm.style.opacity = '0'; 
                setTimeout(() => {
                    searchForm.classList.remove("show");
                    searchForm.style.height = '0';
                }, 300); 
            }
        }
    });

    searchForm.addEventListener("click", function (event) {
        event.stopPropagation();
    });
});

const myAccountLink = document.querySelector('.nav-link.text-light.my-account-link');
const smallNavContent = document.querySelector('.small-nav-content');
const mobileMenuPlaceholder = document.getElementById('mobileMenuPlaceholder');
const smallNav = document.querySelector('.small-nav .navbar-collapse');
const accountRegistration = document.querySelector('.account-login-registration');

function moveSmallNavLinksToMainNav() {
    if (window.innerWidth <= 576) {
       
        myAccountLink.remove();
        mobileMenuPlaceholder.innerHTML = '';
        mobileMenuPlaceholder.appendChild(smallNavContent);

    } else {
        
        mobileMenuPlaceholder.innerHTML = '';
        smallNav.appendChild(smallNavContent);
     
        if (!accountRegistration.contains(myAccountLink)) {
            accountRegistration.prepend(myAccountLink);
        }
        
    }
}

moveSmallNavLinksToMainNav();

window.addEventListener('resize', moveSmallNavLinksToMainNav);

