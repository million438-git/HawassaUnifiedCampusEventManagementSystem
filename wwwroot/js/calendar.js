"use strict";

/* =========================================================
   HUCEMS CALENDAR
   ========================================================= */


/* =========================================================
   EVENT DATA
   ========================================================= */

const campusEvents = [

    {
        id: 1,
        title: "Cyber Security Seminar",
        date: "2026-08-15",
        time: "10:00 AM",
        location: "Computer Science Building",
        organizer: "Cyber Security Department",
        category: "technology",
        description:
            "A seminar about modern cyber security, " +
            "digital safety, ethical hacking and information " +
            "security."
    },

    {
        id: 2,
        title: "Student Orientation",
        date: "2026-08-18",
        time: "9:00 AM",
        location: "Main Auditorium",
        organizer: "Student Affairs",
        category: "academic",
        description:
            "Orientation program for students covering " +
            "academic resources, university services and campus life."
    },

    {
        id: 3,
        title: "Technology Innovation Day",
        date: "2026-08-20",
        time: "11:00 AM",
        location: "Innovation Center",
        organizer: "Innovation Club",
        category: "technology",
        description:
            "Students present innovative software projects, " +
            "research ideas and technology solutions."
    },

    {
        id: 4,
        title: "Campus Football Tournament",
        date: "2026-08-22",
        time: "2:00 PM",
        location: "University Sports Field",
        organizer: "Sports Association",
        category: "sports",
        description:
            "Inter-department football tournament featuring " +
            "student teams from across the university."
    },

    {
        id: 5,
        title: "Career Development Workshop",
        date: "2026-08-25",
        time: "1:00 PM",
        location: "Student Center",
        organizer: "Career Development Office",
        category: "career",
        description:
            "Workshop covering CV preparation, interview skills, " +
            "professional networking and career planning."
    },

    {
        id: 6,
        title: "Cultural Night",
        date: "2026-08-28",
        time: "5:30 PM",
        location: "University Cultural Center",
        organizer: "Cultural Association",
        category: "cultural",
        description:
            "An evening celebrating culture, music, art, dance " +
            "and cultural diversity."
    },

    {
        id: 7,
        title: "Student Community Meetup",
        date: "2026-08-30",
        time: "3:00 PM",
        location: "Campus Garden",
        organizer: "Student Community",
        category: "social",
        description:
            "An informal meetup designed to help students connect, " +
            "share ideas and build friendships."
    },

    {
        id: 8,
        title: "Programming Competition",
        date: "2026-09-03",
        time: "9:00 AM",
        location: "ICT Laboratory",
        organizer: "Computer Science Club",
        category: "technology",
        description:
            "Programming competition where students solve " +
            "algorithmic and software development challenges."
    },

    {
        id: 9,
        title: "Research Presentation",
        date: "2026-09-07",
        time: "10:00 AM",
        location: "Research Hall",
        organizer: "Research Directorate",
        category: "academic",
        description:
            "Students and researchers present selected research " +
            "projects and academic findings."
    },

    {
        id: 10,
        title: "Basketball Championship",
        date: "2026-09-12",
        time: "3:00 PM",
        location: "University Gym",
        organizer: "Sports Association",
        category: "sports",
        description:
            "Campus basketball championship featuring university " +
            "student teams."
    }

];


/* =========================================================
   STATE
   ========================================================= */

let currentDate = new Date();

let selectedDate = new Date();

let filteredEvents = [...campusEvents];


/* =========================================================
   DOM
   ========================================================= */

const calendarGrid =
    document.getElementById("calendarGrid");

const currentMonth =
    document.getElementById("currentMonth");

const previousMonth =
    document.getElementById("previousMonth");

const nextMonth =
    document.getElementById("nextMonth");

const todayButton =
    document.getElementById("todayButton");

const eventSearch =
    document.getElementById("eventSearch");

const categoryFilters =
    document.querySelectorAll(
        ".category-filter input"
    );

const upcomingEvents =
    document.getElementById("upcomingEvents");

const selectedDayTitle =
    document.getElementById("selectedDayTitle");

const selectedEventCount =
    document.getElementById("selectedEventCount");

const selectedDayEvents =
    document.getElementById("selectedDayEvents");


/* =========================================================
   MODAL
   ========================================================= */

const eventModal =
    document.getElementById("eventModal");

const modalOverlay =
    document.getElementById("modalOverlay");

const closeModal =
    document.getElementById("closeModal");

const closeModalButton =
    document.getElementById(
        "closeModalButton"
    );

const modalCategory =
    document.getElementById("modalCategory");

const modalTitle =
    document.getElementById("modalTitle");

const modalDate =
    document.getElementById("modalDate");

const modalTime =
    document.getElementById("modalTime");

const modalLocation =
    document.getElementById("modalLocation");

const modalOrganizer =
    document.getElementById("modalOrganizer");

const modalDescription =
    document.getElementById(
        "modalDescription"
    );

const viewEventButton =
    document.getElementById(
        "viewEventButton"
    );


/* =========================================================
   MONTHS
   ========================================================= */

const months = [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December"
];

const shortMonths = [
    "Jan",
    "Feb",
    "Mar",
    "Apr",
    "May",
    "Jun",
    "Jul",
    "Aug",
    "Sep",
    "Oct",
    "Nov",
    "Dec"
];


/* =========================================================
   INITIALIZE
   ========================================================= */

document.addEventListener(
    "DOMContentLoaded",
    function () {

        renderCalendar();

        renderSelectedDay();

        renderUpcomingEvents();

        setupListeners();

    }
);


/* =========================================================
   LISTENERS
   ========================================================= */

function setupListeners() {

    previousMonth.addEventListener(
        "click",
        function () {

            currentDate.setMonth(
                currentDate.getMonth() - 1
            );

            renderCalendar();

        }
    );


    nextMonth.addEventListener(
        "click",
        function () {

            currentDate.setMonth(
                currentDate.getMonth() + 1
            );

            renderCalendar();

        }
    );


    todayButton.addEventListener(
        "click",
        function () {

            const today =
                new Date();

            currentDate =
                new Date(today);

            selectedDate =
                new Date(today);

            renderCalendar();

            renderSelectedDay();

        }
    );


    eventSearch.addEventListener(
        "input",
        applyFilters
    );


    categoryFilters.forEach(
        function (filter) {

            filter.addEventListener(
                "change",
                applyFilters
            );

        }
    );


    closeModal.addEventListener(
        "click",
        closeEventModal
    );


    closeModalButton.addEventListener(
        "click",
        closeEventModal
    );


    modalOverlay.addEventListener(
        "click",
        closeEventModal
    );


    document.addEventListener(
        "keydown",
        function (event) {

            if (
                event.key === "Escape"
            ) {

                closeEventModal();

            }

        }
    );


    viewEventButton.addEventListener(
        "click",
        function () {

            alert(
                "The full event details page will be connected to the Events module."
            );

        }
    );
}


/* =========================================================
   RENDER CALENDAR
   ========================================================= */

function renderCalendar() {

    calendarGrid.innerHTML = "";


    const year =
        currentDate.getFullYear();

    const month =
        currentDate.getMonth();


    currentMonth.textContent =
        `${months[month]} ${year}`;


    const firstDay =
        new Date(
            year,
            month,
            1
        ).getDay();


    const daysInMonth =
        new Date(
            year,
            month + 1,
            0
        ).getDate();


    const daysInPreviousMonth =
        new Date(
            year,
            month,
            0
        ).getDate();


    /*
     * Previous month.
     */

    for (
        let i = firstDay - 1;
        i >= 0;
        i--
    ) {

        const date =
            new Date(
                year,
                month - 1,
                daysInPreviousMonth - i
            );

        createDay(
            date,
            true
        );

    }


    /*
     * Current month.
     */

    for (
        let day = 1;
        day <= daysInMonth;
        day++
    ) {

        const date =
            new Date(
                year,
                month,
                day
            );

        createDay(
            date,
            false
        );

    }


    /*
     * Next month.
     */

    const cells =
        calendarGrid.children.length;

    const remaining =
        cells % 7 === 0
            ? 0
            : 7 - (cells % 7);


    for (
        let day = 1;
        day <= remaining;
        day++
    ) {

        const date =
            new Date(
                year,
                month + 1,
                day
            );

        createDay(
            date,
            true
        );

    }
}


/* =========================================================
   CREATE DAY
   ========================================================= */

function createDay(
    date,
    otherMonth
) {

    const day =
        document.createElement("div");

    day.className =
        "calendar-day";


    if (otherMonth) {

        day.classList.add(
            "other-month"
        );

    }


    if (
        sameDate(
            date,
            new Date()
        )
    ) {

        day.classList.add(
            "today"
        );

    }


    if (
        sameDate(
            date,
            selectedDate
        )
    ) {

        day.classList.add(
            "selected"
        );

    }


    const number =
        document.createElement("div");

    number.className =
        "calendar-day-number";

    number.textContent =
        date.getDate();


    day.appendChild(number);


    const events =
        getEvents(date);


    if (events.length > 0) {

        const eventContainer =
            document.createElement("div");

        eventContainer.className =
            "calendar-events";


        const max =
            window.innerWidth <= 520
                ? 1
                : 3;


        events
            .slice(0, max)
            .forEach(
                function (event) {

                    const eventElement =
                        document.createElement("div");

                    eventElement.className =
                        `calendar-event ${event.category}`;

                    eventElement.textContent =
                        event.title;

                    eventElement.title =
                        event.title;


                    eventElement.addEventListener(
                        "click",
                        function (e) {

                            e.stopPropagation();

                            openEventModal(
                                event
                            );

                        }
                    );


                    eventContainer.appendChild(
                        eventElement
                    );

                }
            );


        if (
            events.length > max
        ) {

            const more =
                document.createElement("div");

            more.className =
                "more-events";

            more.textContent =
                `+${events.length - max} more`;

            eventContainer.appendChild(
                more
            );

        }


        day.appendChild(
            eventContainer
        );

    }


    day.addEventListener(
        "click",
        function () {

            selectedDate =
                new Date(date);

            renderCalendar();

            renderSelectedDay();

        }
    );


    calendarGrid.appendChild(
        day
    );
}


/* =========================================================
   GET EVENTS
   ========================================================= */

function getEvents(date) {

    const dateString =
        formatDate(date);

    return filteredEvents.filter(
        function (event) {

            return (
                event.date ===
                dateString
            );

        }
    );
}


/* =========================================================
   DATE FORMAT
   ========================================================= */

function formatDate(date) {

    const year =
        date.getFullYear();

    const month =
        String(
            date.getMonth() + 1
        ).padStart(2, "0");

    const day =
        String(
            date.getDate()
        ).padStart(2, "0");


    return `${year}-${month}-${day}`;
}


/* =========================================================
   SAME DATE
   ========================================================= */

function sameDate(
    first,
    second
) {

    return (
        first.getFullYear() ===
        second.getFullYear()
        &&
        first.getMonth() ===
        second.getMonth()
        &&
        first.getDate() ===
        second.getDate()
    );

}


/* =========================================================
   SELECTED DAY
   ========================================================= */

function renderSelectedDay() {

    const events =
        getEvents(
            selectedDate
        );


    selectedDayTitle.textContent =
        selectedDate.toLocaleDateString(
            "en-US",
            {
                weekday: "long",
                month: "long",
                day: "numeric",
                year: "numeric"
            }
        );


    selectedEventCount.textContent =
        `${events.length} ${events.length === 1
            ? "Event"
            : "Events"
        }`;


    selectedDayEvents.innerHTML =
        "";


    if (events.length === 0) {

        selectedDayEvents.innerHTML = `

            <div class="no-events">

                <i class="bi bi-calendar-x"></i>

                <strong>
                    No events scheduled
                </strong>

                <span>
                    There are no events for this day.
                </span>

            </div>

        `;

        return;
    }


    events.forEach(
        function (event) {

            const card =
                document.createElement("div");

            card.className =
                "day-event";


            card.innerHTML = `

                <div class="day-event-time">

                    ${event.time}

                </div>


                <div class="day-event-info">

                    <h3>
                        ${escapeHtml(
                event.title
            )}
                    </h3>

                    <p>

                        <i class="bi bi-geo-alt"></i>

                        ${escapeHtml(
                event.location
            )}

                    </p>

                </div>


                <span
                    class="day-event-category">

                    ${event.category}

                </span>

            `;


            card.addEventListener(
                "click",
                function () {

                    openEventModal(
                        event
                    );

                }
            );


            selectedDayEvents.appendChild(
                card
            );

        }
    );
}


/* =========================================================
   UPCOMING EVENTS
   ========================================================= */

function renderUpcomingEvents() {

    upcomingEvents.innerHTML =
        "";


    const today =
        new Date();

    today.setHours(
        0,
        0,
        0,
        0
    );


    const events =
        filteredEvents
            .filter(
                function (event) {

                    return (
                        new Date(
                            event.date
                        ) >= today
                    );

                }
            )
            .sort(
                function (a, b) {

                    return (
                        new Date(a.date)
                        -
                        new Date(b.date)
                    );

                }
            )
            .slice(0, 5);


    if (events.length === 0) {

        upcomingEvents.innerHTML = `

            <div class="no-events">

                <i class="bi bi-calendar-x"></i>

                <span>
                    No upcoming events
                </span>

            </div>

        `;

        return;
    }


    events.forEach(
        function (event) {

            const date =
                new Date(
                    event.date +
                    "T00:00:00"
                );


            const item =
                document.createElement("div");

            item.className =
                "upcoming-event";


            item.innerHTML = `

                <div class="upcoming-date">

                    <strong>
                        ${date.getDate()}
                    </strong>

                    <span>
                        ${shortMonths[
                date.getMonth()
                ]}
                    </span>

                </div>


                <div class="upcoming-info">

                    <strong>
                        ${escapeHtml(
                    event.title
                )}
                    </strong>

                    <span>
                        ${event.time}
                    </span>

                </div>

            `;


            item.addEventListener(
                "click",
                function () {

                    openEventModal(
                        event
                    );

                }
            );


            upcomingEvents.appendChild(
                item
            );

        }
    );
}


/* =========================================================
   FILTER
   ========================================================= */

function applyFilters() {

    const search =
        eventSearch.value
            .trim()
            .toLowerCase();


    const categories =
        Array.from(
            categoryFilters
        )
            .filter(
                function (filter) {

                    return filter.checked;

                }
            )
            .map(
                function (filter) {

                    return filter.value;

                }
            );


    filteredEvents =
        campusEvents.filter(
            function (event) {

                const matchesSearch =
                    !search
                    ||
                    event.title
                        .toLowerCase()
                        .includes(search)
                    ||
                    event.description
                        .toLowerCase()
                        .includes(search)
                    ||
                    event.location
                        .toLowerCase()
                        .includes(search);


                const matchesCategory =
                    categories.includes(
                        event.category
                    );


                return (
                    matchesSearch &&
                    matchesCategory
                );

            }
        );


    renderCalendar();

    renderSelectedDay();

    renderUpcomingEvents();
}


/* =========================================================
   MODAL
   ========================================================= */

function openEventModal(event) {

    modalCategory.textContent =
        event.category;

    modalTitle.textContent =
        event.title;


    const date =
        new Date(
            event.date +
            "T00:00:00"
        );


    modalDate.textContent =
        date.toLocaleDateString(
            "en-US",
            {
                weekday: "long",
                month: "long",
                day: "numeric",
                year: "numeric"
            }
        );


    modalTime.textContent =
        event.time;

    modalLocation.textContent =
        event.location;

    modalOrganizer.textContent =
        event.organizer;

    modalDescription.textContent =
        event.description;


    eventModal.classList.add(
        "show"
    );


    eventModal.setAttribute(
        "aria-hidden",
        "false"
    );


    document.body.style.overflow =
        "hidden";
}


/* =========================================================
   CLOSE MODAL
   ========================================================= */

function closeEventModal() {

    eventModal.classList.remove(
        "show"
    );


    eventModal.setAttribute(
        "aria-hidden",
        "true"
    );


    document.body.style.overflow =
        "";
}


/* =========================================================
   ESCAPE HTML
   ========================================================= */

function escapeHtml(value) {

    return String(value)
        .replace(
            /&/g,
            "&amp;"
        )
        .replace(
            /</g,
            "&lt;"
        )
        .replace(
            />/g,
            "&gt;"
        )
        .replace(
            /"/g,
            "&quot;"
        )
        .replace(
            /'/g,
            "&#039;"
        );

}