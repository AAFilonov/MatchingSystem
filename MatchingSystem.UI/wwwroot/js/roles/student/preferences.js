let app = new Vue({
    el: '#app',
    data: {
        projectsList: [],
        selectedList: [],
        aviableList: [],
        studentId: null,
        canSave: true,
        currentStageCode: null
    },
    methods: {
        projectIdString(id) {
            return 'proj_' + id;
        },
        projectIdLink(id) {
            return '#proj_' + id;
        },
        toSelected(item) {
            this.selectedList.push(item);
            this.aviableList = this.aviableList.filter(elem => elem !== item);
        },
        toAviable(item) {
            this.aviableList.push(item);
            this.selectedList = this.selectedList.filter(elem => elem !== item);
        },
        toUp(index) {
            if (index === 0) return;
            let tmp = this.selectedList[index];
            Vue.set(this.selectedList, index, this.selectedList[index - 1]);
            Vue.set(this.selectedList, index - 1, tmp);
        }, 
        toDown(index) {
            if (index === this.selectedList.length - 1) return;
            let tmp = this.selectedList[index];
            Vue.set(this.selectedList, index, this.selectedList[index + 1]);
            Vue.set(this.selectedList, index + 1, tmp);
        },
        async savePreferences() {
            let data = new FormData();

            data.append('studentId', this.studentId);
            data.append('selectedList', this.selectedList.map(elem => elem.projectID));

            let request = await fetch(`${params.basePath}/api/Student/set_preferences`, {
                method: 'post',
                body: data
            });
            
            if (request.ok) {
                this.canSave = false;
                DisplayNotification("Сохранено");
            } else {
                DisplayNotification('Произошла ошибка', 'error');
            }
        }
    },
    computed: {
    },
    async mounted() {
        this.studentId = document.getElementById('studentId').value;
        this.currentStageCode = parseInt(this.$refs.currentStageCode.value);
        console.log(this.studentId);
        let request = await fetch(`${params.basePath}/api/Student/get_projects?studentId=${this.studentId}`, {
            method: 'get'
        });

        if (request.ok) {
            this.projectsList = await request.json();
            this.selectedList = this.projectsList.filter(i => i.isSelectedByStudent === true);
            this.aviableList = this.projectsList.filter(i => i.isSelectedByStudent === false);
            if (this.selectedList.length > 0) this.canSave = false;
        } else DisplayNotification("Произошла ошибка при получении списка проектов", "error");
    },
});