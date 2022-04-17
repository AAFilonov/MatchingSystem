let app = new Vue({
    el: '#app',
    data: {
        matchingId: null,
        userId: null,
        btnDisabled: false
    },
    methods: {
        async requestNextStage() {
            this.btnDisabled = true;
            let request = await fetch(`${params.basePath}/api/executive/setNextStage?matchingId=${this.matchingId}&userId=${this.userId}`, {
                method: 'patch'
            });

            if (request.ok) window.location.reload();
            else { 
                DisplayNotification((await request.json()).detail, 'error');
                this.btnDisabled = false;
            }
        },
        async updateEndDateHandler(e) {
            $form = e.target;
            let dateTime = `${$form[0].value} ${$form[1].value}`;
            let data = new FormData();
            data.append('endDate', dateTime);
            data.append('matchingId', this.matchingId);
            
            let response = await fetch(`${params.basePath}/api/executive/setEndDate`, {
                method: 'patch',
                body: data
            });
            
            if (response.ok) window.location.reload();
            else DisplayNotification((await response.json()).detail, 'error');
        }
    },
    mounted() {
        this.matchingId = parseInt(this.$refs.matching.value);
        this.userId = parseInt(this.$refs.userId.value);
    }
})