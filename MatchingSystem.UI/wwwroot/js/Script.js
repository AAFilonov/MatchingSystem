let changeLkWindow = document.querySelector('#js-change-lk-trigger');

if (changeLkWindow !== null) {
    changeLkWindow.addEventListener('click', function (e) {
        e.preventDefault();
        $('#js-change-lk').modal('show');
    });
}

let $stageInfo = document.querySelector('#stage-info-popup');

document.querySelector('#js-stage-info').addEventListener('click', e => {
    e.preventDefault();

    let coords = e.target.getBoundingClientRect();

    console.log(coords);

    $stageInfo.style.top = `calc(${coords.y + coords.height}px + 5vh)`;
    $stageInfo.style.left = `${coords.x}px`;
    $stageInfo.style.transform = 'translateX(-46%)';

    if ($stageInfo.classList.contains('d-none')) {
        $stageInfo.classList.replace('d-none', 'info-show');
    } else if ($stageInfo.classList.contains('info-show')) {
        $stageInfo.classList.replace('info-show', 'info-hide');
    } else if ($stageInfo.classList.contains('info-hide')) {
        $stageInfo.classList.replace('info-hide', 'info-show');
    }
});

let template = new Vue({
    el: '#js-change-lk',
    data: {
        roles: [],
        matchings: [],
        selectedMatching: null,
        userId: null,
    },
    methods: {
        async getMatchings() {
            let matchingResponse = await fetch(`${params.basePath}/api/user/getMatchings?userId=${this.userId}`, {
                method: 'get'
            });
            
            if (matchingResponse.ok) {
                this.matchings = await matchingResponse.json();
            }
        },
        async getRoles(matching = this.selectedMatching) {
            let rolesResponse = await fetch(`${params.basePath}/api/user/getRoles?userId=${this.userId}&matchingId=${matching}`, {
                method: 'get'
            });

            if (rolesResponse.ok) {
                this.roles = await rolesResponse.json();
            }
        },
        async selectChangeHandler(e) {
            let matching = parseInt(e.target.value);
            await this.getRoles(matching);
        }
    },
    async mounted() {
        this.selectedMatching = parseInt(document.querySelector('#matchingId').value);
        this.userId = parseInt(document.querySelector('#userId').value);
        await this.getMatchings();
        await this.getRoles();
    }
});