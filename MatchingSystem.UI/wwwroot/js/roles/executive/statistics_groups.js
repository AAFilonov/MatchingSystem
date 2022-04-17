let app = new Vue({
    el: '#app',
    data: {
        matchingId: null,
        currentStage: null,
        statistics: []
    },
    methods: {
        async initialize() {
            let response = await fetch(`${params.basePath}/api/Statistics/getStatisticsGroups?matchingId=${this.matchingId}`, {
                method: 'GET'
            });
            
            if (response.ok) {
                this.statistics = await response.json();
                //console.log(this.statistics);
            } else DisplayNotification('Произошла внутренняя ошибка сервера.', 'error');
        },
        calculatePercentage(item){

            return item.statPercentage*100;
        }
    },
    async mounted() {
        this.matchingId = matchingId;
        this.currentStage = currentStage;
        
        await this.initialize();
    }
});