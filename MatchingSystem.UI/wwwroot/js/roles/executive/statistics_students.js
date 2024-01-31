let app = new Vue({
    el: '#app',
    data: {
        matchingId: null,
        currentStage: null,
        student: null,
        projects: [],
        statistics: []
    },
    methods: {
        async initialize() {

            let response = await fetch(`${params.basePath}/api/Statistics/getStatisticsStudents?matchingId=${this.matchingId}&currentStage=${this.currentStage}`, {
                method: 'GET'
            });

            if (response.ok) {
                this.statistics = await response.json();
            } else DisplayNotification('Произошла внутренняя ошибка сервера.', 'error');
        },
        async projectsByStudentInfoWindow(item) {
            this.student = item;
            let request = await fetch(`${params.basePath}/api/Statistics/getStatisticsStudentsProjects?matchingId=${this.matchingId}&studentId=${this.student.studentId}`);
            if (request.ok) {

                let result = await request.json();
                this.projects = result;
                setTimeout(() => {
                    $('#projectsByStudent').modal('show');
                }, 100);
            }
        },
    },
    async mounted() {
        this.matchingId = matchingId;
        this.currentStage = currentStage;

        await this.initialize();
    }
});
