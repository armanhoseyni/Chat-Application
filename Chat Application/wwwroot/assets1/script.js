const privateDropdowns = document.querySelectorAll(".dropdown-menu");

function toggleDropdown() {
    if (privateDropdowns[0].classList.contains("hidden")) {
        privateDropdowns[0].classList.remove("hidden")
    } else {
        privateDropdowns[0].classList.add("hidden")
    }
}

function toggleDropdown2() {
    if (privateDropdowns[1].classList.contains("hidden")) {
        privateDropdowns[1].classList.remove("hidden")
    } else {
        privateDropdowns[1].classList.add("hidden")
    }
}