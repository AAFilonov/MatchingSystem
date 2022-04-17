let app = new Vue({
    el: '#app',
    data: {
        matchingId: null,
        currentStage: null,
        tutor: null,
        projects: [],
        projectsAllocated: [],
        statistics: []
    },
    methods: {
        async initialize() {
            let response = await fetch(`${params.basePath}/api/Statistics/getStatisticsTutors?matchingId=${this.matchingId}&currentStage=${this.currentStage}`, {
                method: 'GET'
            });
            
            if (response.ok) {
                this.statistics = await response.json();
                //console.log(this.statistics);
            } else DisplayNotification('Произошла внутренняя ошибка сервера.', 'error');
        },
         async projectsByTutorInfoWindow(item) {
             this.tutor = item;

        
             //TODO переписать на запрос статистики  по проектам  по этапу
             let request = await fetch(`${params.basePath}/api/Statistics/getStatisticsTutorProjects?matchingId=${this.matchingId}&tutorId=${this.tutor.tutorID}`);
             if (request.ok) {
               
                 let result = await request.json();
                 console.log(result);
                 this.projects = result;
                 setTimeout(() => {
                     $('#projectsByTutor').modal('show');
                 }, 100);
             } else {
                 DisplayNotification('Произошла ошибка при получении данных', 'error');
             }
          
         },
        async tutorProjectsAllocatedWindow(item) {
            this.tutor = item;

          

            let request = await fetch(`${params.basePath}/api/Statistics/getStatisticsTutorProjectAllocated?tutorId=${this.tutor.tutorID}&matchingId=${this.matchingId}`);
            if (request.ok) {

                let result = await request.json();
                console.log(result)
                this.projectsAllocated = result;
                setTimeout(() => {
                    $('#tutorProjectsAllocated').modal('show');
                }, 100);
            } else {
                DisplayNotification('Произошла ошибка при получении данных', 'error');
            }

        }
    },
    async mounted() {
        this.matchingId = matchingId;
        this.currentStage = currentStage;
      
        await this.initialize();
    },
   
});
