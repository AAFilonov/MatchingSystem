let app = new Vue({
    el: '#app',
    data: {
        allocations: [],
        selectedMatching: null
    },
    methods: {
        async initialize() {
            let response = await fetch(`${params.basePath}/api/allocation/getFinalAllocations?selectedMatching=${this.selectedMatching}`);
            
            if (response.ok) {
                this.allocations = await response.json();
                
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
        this.selectedMatching = selectedMatching;
        
        await this.initialize();
    }
});