function DisplayNotification(message, type = 'success') {
    let element = document.createElement('div');
    element.classList.add('alert', 'alert-dismissible', (type === 'success')? 'alert-success': 'alert-danger', 'notification');
    element.innerHTML = `<button type="button" class="close" data-dismiss="alert">&times;</button>
        ${message}`;
    element.addEventListener('animationend', e => element.remove());
    document.querySelector('.notification-bar').appendChild(element);
}

async function CheckNotificationsByExecutive(text) {
    let $form = document.querySelector('#metadata');
    let result = await fetch(`${params.basePath}/api/Notification/get_notifications?userId=${$form[0].value}&matchingId=${$form[1].value}`, {
        method: 'get',
    });

    if (result.ok) {
        let _result = await result.json();
        if (_result.count > 0)
            DisplayNotification(text);
    }
}

async function CheckNotificationsByTutor(text, tutorId, userId, matching) {
    let result = await fetch(`${params.basePath}/api/Notification/getNotificationsByTutor?tutorId=${tutorId}&userId=${userId}&matchingId=${matching}`, {
        method: 'get',
    });

    if (result.ok) {
        let _result = await result.json();
        if (_result.count > 0)
            DisplayNotification(text);
    }
}