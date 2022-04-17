let app = new Vue({
    el: '#app',
    data: {
        tutorId: null,
        selectedMatching: null,
        currentStage: null,
        generalQuota: null,
        totalInQuota: null,
        choices: [],
        studentInfoWindow: null,
        student: {},
    },
    methods: {
        choiceClickHandler(item) {
            item.isInQuota = !item.isInQuota;
            // увеличиваем или уменьшаем общий счетчик количества студентов в квоте 
            item.isInQuota? this.totalInQuota++ : this.totalInQuota--;
            
            this.choices.forEach(e => {
                e.aviableQty = this.aviableQtyPerProject(e);
            });
        },
        aviableQtyPerProject(project) {
            let aviableTotal = this.generalQuota - this.totalInQuota;
            let projectQty = project.qty || this.generalQuota;
            let aviableForProject = projectQty - project.choices.reduce((sum, c) => { return c.isInQuota? ++sum : sum }, 0);
            if (aviableForProject <= aviableTotal) {
                if (project.choices.length <= projectQty) {
                    if (project.choices.filter(x => x.isInQuota === true).length === project.choices.length) return  0;
                    let tmp = project.choices.filter(e => e.isInQuota === true).length;
                    let ret = project.choices.length - tmp;
                    if (ret >= aviableForProject) return aviableForProject;
                    return ret;
                }
                return aviableForProject;
            }
            else {
                let res = aviableForProject - (aviableForProject - aviableTotal);
                if (project.choices.length <= projectQty) {
                    if (project.choices.filter(x => x.isInQuota === true).length === project.choices.length) return  0;
                    let tmp = project.choices.filter(e => e.isInQuota === true).length;
                    let ret = project.choices.length - tmp;
                    if (ret >= res) return res;
                    return ret;
                }
                return res;
            }
        },
        async buttonInfoHandler(studentId) {
            let response = await fetch(`${params.basePath}/api/student/getStudentInfo?studentId=${studentId}`, {
                method: 'get'
            });
            $(this.studentInfoWindow).modal('show');
            if (response.ok) this.student = await response.json();
        },
        closeWindowHandler() {
            $(this.studentInfoWindow).modal('hide');
            this.student = {};
        },
        moveUpHandler(choice, project) {
            let index = project.choices.indexOf(choice);

            if (index > 0) {
                let tmp = project.choices[index];
                Vue.set(project.choices, index, project.choices[index - 1]);
                Vue.set(project.choices, index - 1, tmp);
            }
        },
        moveDownHandler(choice, project) {
            let index = project.choices.indexOf(choice);
            
            if (index < project.choices.length - 1) {
                let tmp = project.choices[index];
                Vue.set(project.choices, index, project.choices[index + 1]);
                Vue.set(project.choices, index + 1, tmp);
            }
        },
        async closeProjectHandler(project) {
            await this.saveChoiceHandler();
            let response = await fetch(`${params.basePath}/api/projects/closeProject?projectId=${project.projectID}&tutorId=${this.tutorId}`, { 
                method: 'patch',
            });
            
            if (response.ok) project.projectIsClosed = true;
        },
        async saveChoiceHandler() {
            this.choices.forEach(elem => {
                elem.choices.forEach((x, i) => x.sortOrderNumber = i + 1);
            });
            
            let data = [];
            
            this.choices.forEach(elem => {
                elem.choices.forEach(e => {
                    if (e.choiceID === null) return;
                    data.push({
                        choiceId: e.choiceID,
                        sortOrderNumber: e.sortOrderNumber,
                        isInQuota: e.isInQuota
                    })
                });
            });
            
            let response = await fetch(`${params.basePath}/api/tutor/saveChoice?tutorId=${this.tutorId}`,{
                method: 'patch',
                body: JSON.stringify(data),
                headers: {
                    "Content-Type": "application/json"
                }
            });
            
            if (response.ok) DisplayNotification('Выбор сохранен');
            else DisplayNotification((await response.json()).detail, 'error');
        },
        canSaveGlobal() {
            for (let i = 0; i < this.choices.length; i++)
                if (this.choices[i].aviableQty > 0) return false;
            return true;
        }
    },
    computed: {
        studentName () {
            if (this.student === null) return '';
            return `${this.student.surname || ''} ${this.student.name || ''} ${this.student.patronimic || ''}`;
        }
    },
    async mounted() {
        this.tutorId = tutorId;
        this.selectedMatching = selectedMatching;
        this.currentStage = currentStage;

        let request = await fetch(`${params.basePath}/api/Tutor/getChoice?tutorId=${this.tutorId}`);
        
        if (request.ok) {
            let result = await request.json();
            
            this.choices = result.choiceDatas;
            this.generalQuota = result.commonQuota;
            this.studentInfoWindow = this.$refs.infoModal;
            this.choices.forEach(element => {
                
                element.choices.forEach(e => {
                    if (e.isInQuota) this.totalInQuota++
                });
                
                element.qtyDescription = element.qty || 'Не важно';
            });
            this.choices.forEach(project => {
                if (project.choices.length === 1 && project.choices[0].choiceID === null) {
                    project.choices.pop();
                }
            })
            this.choices.forEach(e => {
                e.aviableQty = this.aviableQtyPerProject(e);
            });
        } else {
            DisplayNotification((await request.json()).detail, 'error');
        }
    }
});