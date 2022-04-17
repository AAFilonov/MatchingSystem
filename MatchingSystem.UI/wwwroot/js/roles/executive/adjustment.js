let app = new Vue({
    el: '#app',
    data: {
        userId: null,
        matchingId: null,
        currentStageCode: null,
        allocations: [],
        tutors: [],
        projects: []
    },
    methods: {
        async initialize() {
            let response = await fetch(`${params.basePath}/api/executive/getAllocation?userId=${this.userId}&matchingId=${this.matchingId}`, {
                method: 'get'
            });
            
            if (response.ok) {
                let result = await response.json();
                this.allocations = result.allocations;
                this.tutors = result.tutors;
                this.projects = result.projects;
            }
                
            else DisplayNotification((await response.json()).detail, 'error');
        },
        async saveAllocationClickHandler() {
            let students = this.allocations.filter(x => x.isAllocated === false && x.projectID !== null && x.tutorID !== null);
            let data = {
                userId: this.userId,
                matchingId: this.matchingId,
                adjustment: students
            }
            
            let response = await fetch(`${params.basePath}/api/executive/setAllocation`, {
                method: 'patch',
                body: JSON.stringify(data),
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            
            if (response.ok) window.location.reload();
            else { 
                DisplayNotification((await response.json()).detail, 'error');
            }
        },
        sortAllocationsHandler(key) {
            if (key === 'group')
            {
                this.allocations = this.allocations.sort(function (a, b) {
                    if (a.groupName < b.groupName) return -1;
                    else if (a.groupName === b.groupName) return 0;
                    else return 1;
                });
            }
            else if (key === 'student') {
                this.allocations = this.allocations.sort(function (a, b) {
                    if (a.studentNameAbbreviation < b.studentNameAbbreviation) return -1;
                    else if (a.studentNameAbbreviation === b.studentNameAbbreviation) return 0;
                    else return 1;
                });
            } else if (key === 'tutor') {
                this.allocations = this.allocations.sort(function (a, b) {
                    if (a.tutorNameAbbreviation < b.tutorNameAbbreviation) return -1;
                    else if (a.tutorNameAbbreviation === b.tutorNameAbbreviation) return 0;
                    else return 1;
                });
            } else if (key === 'project') {
                this.allocations = this.allocations.sort(function (a, b) {
                    if (a.projectName < b.projectName) return -1;
                    else if (a.projectName === b.projectName) return 0;
                    else return 1;
                });
            }
        }
    },
    async mounted() {
        this.userId = userId;
        this.matchingId = matchingId;
        this.currentStageCode = currentStageCode;
        
        await this.initialize();
    }
});