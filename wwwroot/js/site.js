/* =========================================================
   HAWASSA UNIFIED CAMPUS EVENT MANAGEMENT SYSTEM
   GLOBAL WEBSITE JAVASCRIPT
   ========================================================= */

document.addEventListener("DOMContentLoaded", function () {

    console.log(
        "Hawassa Unified Campus Event Management System loaded."
    );


    /* =====================================================
       CURRENT YEAR
       ===================================================== */

    const yearElements =
        document.querySelectorAll(".current-year");

    yearElements.forEach(function (element) {

        element.textContent =
            new Date().getFullYear();

    });


    /* =====================================================
       ACTIVE NAVIGATION
       ===================================================== */

    const currentPath =
        window.location.pathname.toLowerCase();

    const navLinks =
        document.querySelectorAll(".navbar-nav .nav-link");

    navLinks.forEach(function (link) {

        const href =
            link.getAttribute("href");

        if (!href || href === "#") {
            return;
        }

        const linkPath =
            new URL(
                href,
                window.location.origin
            ).pathname.toLowerCase();

        if (
            linkPath !== "/" &&
            currentPath.startsWith(linkPath)
        ) {
            link.classList.add("active");
        }

    });


    /* =====================================================
       AUTO-HIDE ALERTS
       ===================================================== */

    const alerts =
        document.querySelectorAll(
            ".alert[data-auto-hide]"
        );

    alerts.forEach(function (alert) {

        setTimeout(function () {

            alert.style.transition =
                "opacity 0.4s ease";

            alert.style.opacity = "0";

            setTimeout(function () {

                alert.remove();

            }, 400);

        }, 4000);

    });


    /* =====================================================
       DELETE CONFIRMATION
       ===================================================== */

    const deleteButtons =
        document.querySelectorAll(
            ".delete-confirm"
        );

    deleteButtons.forEach(function (button) {

        button.addEventListener(
            "click",
            function (event) {

                const confirmed =
                    window.confirm(
                        "Are you sure you want to delete this item?"
                    );

                if (!confirmed) {

                    event.preventDefault();

                }

            }
        );

    });


    /* =====================================================
       PASSWORD SHOW / HIDE
       ===================================================== */

    const passwordButtons =
        document.querySelectorAll(
            ".password-toggle"
        );

    passwordButtons.forEach(function (button) {

        button.addEventListener(
            "click",
            function () {

                const target =
                    button.getAttribute(
                        "data-target"
                    );

                if (!target) {
                    return;
                }

                const input =
                    document.querySelector(target);

                if (!input) {
                    return;
                }

                if (
                    input.type === "password"
                ) {

                    input.type = "text";

                    button.textContent =
                        "Hide";

                } else {

                    input.type = "password";

                    button.textContent =
                        "Show";

                }

            }
        );

    });


    /* =====================================================
       MOBILE NAVBAR
       ===================================================== */

    const mobileNavLinks =
        document.querySelectorAll(
            ".navbar-collapse .nav-link"
        );

    mobileNavLinks.forEach(function (link) {

        link.addEventListener(
            "click",
            function () {

                const navbar =
                    document.querySelector(
                        ".navbar-collapse"
                    );

                const toggle =
                    document.querySelector(
                        ".navbar-toggler"
                    );

                if (
                    navbar &&
                    toggle &&
                    navbar.classList.contains("show")
                ) {

                    toggle.click();

                }

            }
        );

    });


    /* =====================================================
       BACK TO TOP BUTTON
       ===================================================== */

    const backToTop =
        document.getElementById(
            "backToTop"
        );

    if (backToTop) {

        window.addEventListener(
            "scroll",
            function () {

                if (window.scrollY > 400) {

                    backToTop.style.display =
                        "block";

                } else {

                    backToTop.style.display =
                        "none";

                }

            }
        );


        backToTop.addEventListener(
            "click",
            function () {

                window.scrollTo({
                    top: 0,
                    behavior: "smooth"
                });

            }
        );

    }


    /* =====================================================
       SEARCH CLEAR BUTTON
       ===================================================== */

    const searchClearButtons =
        document.querySelectorAll(
            ".search-clear"
        );

    searchClearButtons.forEach(function (button) {

        button.addEventListener(
            "click",
            function () {

                const target =
                    button.getAttribute(
                        "data-target"
                    );

                const input =
                    document.querySelector(
                        target
                    );

                if (input) {

                    input.value = "";

                    input.focus();

                }

            }
        );

    });


    /* =====================================================
       DISABLE BUTTON AFTER FORM SUBMIT
       ===================================================== */

    const forms =
        document.querySelectorAll(
            "form[data-disable-submit]"
        );

    forms.forEach(function (form) {

        form.addEventListener(
            "submit",
            function () {

                const submitButton =
                    form.querySelector(
                        'button[type="submit"]'
                    );

                if (!submitButton) {
                    return;
                }

                submitButton.disabled =
                    true;

                submitButton.dataset.originalText =
                    submitButton.textContent;

                submitButton.textContent =
                    "Please wait...";

            }
        );

    });


    /* =====================================================
       GLOBAL TOOLTIP INITIALIZATION
       Bootstrap 5
       ===================================================== */

    if (
        typeof bootstrap !== "undefined"
    ) {

        const tooltipElements =
            document.querySelectorAll(
                '[data-bs-toggle="tooltip"]'
            );

        tooltipElements.forEach(
            function (element) {

                new bootstrap.Tooltip(
                    element
                );

            }
        );

    }


    /* =====================================================
       GLOBAL CONFIRMATION DATA ATTRIBUTE
       ===================================================== */

    const confirmElements =
        document.querySelectorAll(
            "[data-confirm]"
        );

    confirmElements.forEach(function (element) {

        element.addEventListener(
            "click",
            function (event) {

                const message =
                    element.getAttribute(
                        "data-confirm"
                    );

                if (
                    message &&
                    !window.confirm(message)
                ) {

                    event.preventDefault();

                }

            }
        );

    });

});