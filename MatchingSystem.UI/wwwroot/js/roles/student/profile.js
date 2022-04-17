document.addEventListener('DOMContentLoaded', () => {
    let profileEditWindow = document.querySelector('#js-profile-edit-trigger');
    let profileEditForm = document.querySelector('#js-profile-edit-modal form');

    if (profileEditWindow !== null) {
        profileEditWindow.addEventListener('click', async function (e) {
            e.preventDefault();

            let request = await fetch(`${params.basePath}/api/Student/get_selected_info/` + e.target.dataset.studentId);

            if (request.ok) {
                let result = await request.json();

                document.querySelectorAll('input[name="tech"]').forEach(elem => {
                    if (result.technologies.find(item => item.technologyCode == elem.value)) elem.setAttribute('checked', true);
                });

                document.querySelectorAll('input[name="workDirection"]').forEach(elem => {
                    if (result.workDirections.find(item => item.directionCode == elem.value)) elem.setAttribute('checked', true);
                });

                document.querySelector('textarea[name="info"]').value = result.info;

                $('#js-profile-edit-modal').modal('show');
            }
        });
    }


    if (profileEditForm !== null) {
        profileEditForm.addEventListener('submit', async function (e) {
            e.preventDefault();

            let data = new FormData(e.target);

            let request = await fetch(`${params.basePath}/api/student/edit_profile`, {
                method: 'patch',
                body: data
            });

            if (request.status === 204) window.location.reload();
        })
    }

    let app = new Vue({
        el: '#app',
        data: {
            studentId: null,
            currentStageCode: null,
            allocatedProject: {},
            currentMatching: null,
            selectedGroup: null
        },
        async mounted() {
            this.studentId = studentId;
            this.currentStageCode = currentStageCode;
            this.currentMatching = selectedMatching;
            await loadSelectedParams();

            if (this.currentStageCode > 5) {
                let response = await fetch(`${params.basePath}/api/student/getAllocatedProject?studentId=${this.studentId}`)

                if (response.ok) this.allocatedProject = await response.json();
            }
        }
    });
});

async function loadSelectedParams() {
    let request = await fetch(`${params.basePath}/api/Student/get_selected_info?studentId=` + studentId);

    if (request.ok) {
        let result = await request.json();


        document.querySelectorAll('input[name="tech"]').forEach(elem => {
            if (result.technologies.find(item => item.technologyCode == elem.value)) elem.setAttribute('checked', true);
        });

        document.querySelectorAll('input[name="workDirection"]').forEach(elem => {
            if (result.workDirections.find(item => item.directionCode == elem.value))
                elem.setAttribute('checked', "true");
        });

        document.querySelector('textarea[name="info"]').value = result.info;

        let info2 = document.querySelector(`select[name="info2"]`);
        info2.value = result.info2;
        console.log(info2);
    }
}