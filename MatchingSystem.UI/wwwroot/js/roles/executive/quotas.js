document.querySelectorAll('.js-accept-request-btn').forEach(elem => {
    elem.addEventListener('click', async function (e) {
        e.preventDefault();

        let data = new FormData();
        data.append('quotaId', e.target.dataset.quotaId);
        data.append('action', 'accept');

        let result = await fetch(`${params.basePath}/api/Quotas/process_request`, {
            method: 'patch',
            body: data
        });

        if (result.status === 204) window.location.reload();
    })
});

document.querySelectorAll('.js-decline-request-btn').forEach(elem => {
    elem.addEventListener('click', async function (e) {
        e.preventDefault();

        let data = new FormData();
        data.append('quotaId', e.target.dataset.quotaId);
        data.append('action', 'decline');

        let result = await fetch(`${params.basePath}/api/quotas/process_request`, {
            method: 'patch',
            body: data
        });

        if (result.status === 204) window.location.reload();
    })
});