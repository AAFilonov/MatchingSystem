let app = new Vue({
    el: '#app',
    data: {
        tutorId: null,
        matching: null,
        stageTypeCode: null,
        historyList: [],
        projects: [],
        projectsToChange: [],
        commonQuota: null,
        newQuota: null,
        message: null,
        totalDelta: 0,
        stageName: ''
    },
    methods: {
        async initialize() {
            let request = await fetch(`${params.basePath}/api/quotas/tutor/initialize` +
            `?tutorId=${this.tutorId}` +
            `&stageTypeCode=${this.stageTypeCode}`, {
                method: 'get'
            });
            
            if (request.ok) {
                let result = await request.json();
                this.commonQuota = result.commonQuota;
                this.historyList = result.history;
                this.projects = result.projects;
                
                this.projects.forEach(e => e.delta = 0)
            }
            
            return request.ok;
        },
        addProjectHandler(item) {
            this.projectsToChange.push(item);
            this.projects = this.projects.filter(i => i !== item);
        },
        removeFromProjectToChangeHandler(item) {
            this.projects.push(item);
            this.projectsToChange = this.projectsToChange.filter(i => i !== item);
        },
        closeWindowHandler() {
            this.projects.concat(this.projectsToChange);
            this.projectsToChange = [];
            this.totalDelta = 0;
        },
        async sendRequestHandler() {
            let response;
            if (this.stageTypeCode === 4) {
                let data = this.projectsToChange.map(i => {
                    return {
                        projectId: i.projectID,
                        quota: parseInt(i.delta)
                    }
                });
                response = await fetch(`${params.basePath}/api/quotas/sendRequestForLastStage`, {
                    method: 'post',
                    body: JSON.stringify({
                        deltas: data,
                        message: this.message,
                        tutorId: this.tutorId,
                        newQuota: this.totalDelta + this.commonQuota
                    }),
                    headers: {
                        "Content-Type": "application/json"
                    }
                });
            } else if (this.stageTypeCode < 4) {
                response = await fetch(`${params.basePath}/api/quotas/send_request`, {
                    method: 'post',
                    body: JSON.stringify({
                        message: this.message,
                        tutorId: this.tutorId,
                        newQuota: parseInt(this.newQuota)
                    }),
                    headers: {
                        "Content-Type": "application/json"
                    }
                });
            }
            
            if (response.status < 400) {
                this.historyList.unshift({
                    stageTypeName_ru: this.stageName,
                    isNew: true,
                    quotaStateName_ru: 'отправлен запрос',
                    qty: this.newQuota || (this.commonQuota + this.totalDelta)
                });
                DisplayNotification('Запрос ответственному был отправлен');
            } else {
                DisplayNotification((await response.json()).detail, 'error');
            }
            
            $('#request-modal').modal('hide');
        },
        numericUpDownChangeHandler() {
            let controls = this.$refs.requestModal.querySelectorAll('input[type="number"]');
            this.totalDelta = [].reduce.call(controls, (s, c) => {
                return s + parseInt(c.value);
            }, 0);
        }
    },
    async mounted() {
        this.tutorId = parseInt(this.$refs.tutorId.value);
        this.matching = parseInt(this.$refs.matching.value);
        this.stageTypeCode = parseInt(this.$refs.stageTypeCode.value);
        this.stageName = this.$refs.stageName.value;
        
        await this.initialize();
    }
})