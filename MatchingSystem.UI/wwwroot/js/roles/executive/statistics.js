let app = new Vue({
    el: '#app',
    data: {
        matchingId: null,
        currentStage: null,
        statistics: []
    },
    methods: {
        async initialize() {
            let response = await fetch(`${params.basePath}/api/executive/getStatisticsMain?matchingId=${this.matchingId}&currentStage=${this.currentStage}`, {
                method: 'GET'
            });
            
            if (response.ok) {
                this.statistics = await response.json();
            } else DisplayNotification('Произошла внутренняя ошибка сервера.', 'error');
        }
    },
    async mounted() {
        this.matchingId = matchingId;
        this.currentStage = currentStage;
        
        await this.initialize();
    }
});