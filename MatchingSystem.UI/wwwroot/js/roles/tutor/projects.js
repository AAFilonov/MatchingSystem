let app = new Vue({
    el: '#app',
    data: {
        projects: [],
        techList: [],
        directionList: [],
        commonQuota: null,
        aviableQty: null,
        groupList: [],
        isReady: 1,
        tutorId: null,
        selectedMatching: null,
        edit: null,
        currentStageCode: null,
        matchingTypeCode: null,
        formErrors: []
    },
    computed: {
        qtyPerProject() {
            if (this.projects.length === 0) return 0;

            let result = [];

            for (let i = 0; i <= this.commonQuota; i++) {
                result.push(i);
            }

            result.push("Не важно");

            return result;
        },
    },
    methods: {
        isBachelorMatching() {
            return this.matchingTypeCode === '1';
        },
        isMagisterMatching() {
            return this.matchingTypeCode === '2';
        },
        async initialize() {
            let request = await fetch(`${params.basePath}/api/Projects/get_projects_data?tutorId=${this.tutorId}&selectedMatching=${this.selectedMatching}`);

            if (request.ok) {
                let result = await request.json();

                this.projects = result.projects;
                this.groupList = result.groups;
                this.techList = result.technology;
                this.directionList = result.workDirections;
                this.isReady = result.isReady;
                this.commonQuota = result.commonQuota;

                this.projects.forEach(elem => {
                    if (elem.technologiesName_List === null) elem.technologiesName_List = '';
                    if (elem.workDirectionsName_List === null) elem.workDirectionsName_List = '';
                    if (elem.availableGroupsName_List === null) elem.availableGroupsName_List = '';
                    if (elem.qty === null) elem.qty = 'Не важно'
                })
            } else {
                DisplayNotification('Произошла ошибка при получении данных', 'error');
            }
        },
        editWindow(item) {
            this.edit = item;

            setTimeout(() => {
                $('#edit-project').modal('show');
            }, 100);
        },
        closeEdit() {
            $('#edit-project').modal('hide');
            setTimeout(() => this.edit = null, 500);
        },
        async createProject(e) {
            this.checkErrors();
            if (this.formErrors.length > 0) return;

            let $form = e.target;
            let data = new FormData(e.target);
            data.append('tutorId', this.tutorId);

            let request = await fetch(`${params.basePath}/api/Projects/tutor/add_project`, {
                method: 'post',
                body: data
            });

            if (request.ok) {
                location.reload();
            } else DisplayNotification('Произошла ошибка при обработке запроса', 'error');
        },
        async saveEdit(e) {
            this.checkErrors();
            if (this.formErrors.length > 0) return;

            let $form = e.target;
            let data = new FormData(e.target);
            data.append('tutorId', this.tutorId);

            let request = await fetch(`${params.basePath}/api/Projects/tutor/edit_project`, {
                method: 'put',
                body: data
            });

            if (request.ok) {
                location.reload();
            } else DisplayNotification('Произошла ошибка при обработке запроса', 'error');
        },
        async setReady() {
            let request = await fetch(`${params.basePath}/api/tutor/set_ready?tutorId=${this.tutorId}`, {
                method: 'patch'
            });

            if (request.ok) {
                this.isReady = 1;
            } else DisplayNotification('Произошла ошибка при обработке запроса', 'error');
        },
        async deleteProject(item) {
            let requet = await fetch(`${params.basePath}/api/Projects/delete?projectId=${item.projectID}`, {
                method: 'delete'
            });

            if (requet.status === 204) {
                this.projects = this.projects.filter(i => i !== item);
            } else DisplayNotification('Произошла ошибка при обработке запроса', 'error');
        },
        async saveNewQuota(e) {
            let data = new FormData();
            data.append('quota', e.target[0].value);
            data.append('tutorId', this.tutorId);
            data.append('projectId', this.edit.projectID);
            data.append('matching', this.selectedMatching);
            let request = await fetch(`${params.basePath}/api/Projects/editQuotaPerProject`, {
                method: 'patch',
                body: data
            });

            if (request.ok) window.location.reload();
            else DisplayNotification('Произошла ошибка при обработке запроса', 'error');
        },
        async saveQuotaDelta(e) {
            let data = new FormData();
            data.append('quota', e.target[0].value);
            data.append('tutorId', this.tutorId);
            data.append('projectId', this.edit.projectID);
            data.append('matching', this.selectedMatching);

            let request = await fetch(`${params.basePath}/api/Projects/editQuotaPerProject`, {
                method: 'patch',
                body: data
            });

            if (request.ok) window.location.reload();
            else DisplayNotification('Произошла ошибка при обработке запроса', 'error');
        },
        checkErrors() {
            this.formErrors = [];
            let groups = document.querySelectorAll('#aviableGroups input[type="checkbox"]');

            let countGroups = [].reduce.call(groups, (sum, c) => {
                return c.checked ? ++sum : sum
            }, 0);
            if (countGroups === 0) this.formErrors.push('Вы должны выбрать хотя бы одну группу.');
        },
        resetForm(e) {
            this.$refs.form.reset();
        }
    },
    async mounted() {
        this.tutorId = parseInt(document.querySelector('input[name="tid"]').value);
        this.selectedMatching = parseInt(document.querySelector('input[name="matching"]').value);
        this.currentStageCode = parseInt(this.$refs.currentStageCode.value);
        this.matchingTypeCode = this.$refs.matchingTypeCode.value;


        await this.initialize();

        let usedQuota = this.projects.reduce((sum, c) => {
            return (c.qty !== 'Не важно') ? sum += c.qty : sum
        }, 0);
        this.aviableQty = this.commonQuota - usedQuota;
    }

});

function fillArray(dest, number) {
    for (let i = 0; i <= number; i++) {
        dest.push(i);
    }
    dest.push('Не важно');
}

function findQtySum(source) {
    return source.reduce((sum, current) => {
        if (current.qty === 'Не важно') return sum + 0;
        return sum + current.qty;
    }, 0);


}
