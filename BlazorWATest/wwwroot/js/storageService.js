let db = '';

initDb();

function initDb() {
    if (!window.indexedDB) {
        console.log(`Your browser doesn't support IndexedDB`);
        return;
    }

    const request = indexedDB.open('BlazorAuth', 1);
    
    request.onupgradeneeded = (event) => {
        db = event.target.result;

        let store = db.createObjectStore('Users', {
            autoIncrement: true
        });

        let index = store.createIndex('email', 'email', {
            unique: true
        });
    };

    request.onsuccess = (event) => {
        db = event.target.result;

        insertUser({
            email: 'john@outlook.com',
            password: 'Test12345!',
            firstName: 'John',
            lastName: 'Doe',
            position: 'admin'
        });

        insertUser({
            email: 'jane@gmail.com',
            password: 'Test978456!',
            firstName: 'Jane',
            lastName: 'Doe',
            position: 'developer'
        });

        insertUser({
            email: 'test@test.kz',
            password: 'Test1234!',
            firstName: 'Test',
            lastName: 'Testov',
            position: 'developer'
        });        
    };
}

function insertUser(user) {
    const txn = db.transaction('Users', 'readwrite');
    const store = txn.objectStore('Users');
    let query = store.put(user);
    
    query.onsuccess = function (event) {
        console.log(event);
    };
    
    query.onerror = function (event) {
        console.log(event.target.errorCode);
    }
    
    txn.oncomplete = function () {
        
    };
}

function getUser(email, password) {
    const txn = db.transaction('Users', 'readonly');
    const store = txn.objectStore('Users');
    let user = '';
    
    const index = store.index('email');
    let query = index.get(email);
    
    query.onsuccess = (event) => {
        user = query.result;        
        var resultPassword = user.password;
        if (resultPassword == password) {
            window.localStorage.setItem('User', JSON.stringify(user));
        } else {
            console.log('Password not valid!');
        }
        console.log(query.result.email + ' '+query.result.password); 
    };
    
    query.onerror = (event) => {
        console.log(event.target.errorCode);
    }
    
    txn.oncomplete = function () {                
    };
}

function showAlert(message) {
    
    alert(message);
}