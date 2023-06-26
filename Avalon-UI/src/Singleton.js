// Singleton.js
class Singleton {
    constructor() {
        this.name = '';
    }

    getName() {
        return this.name;
    }

    setName(newName) {
        this.name = newName;
    }

    static getInstance() {
        if (!Singleton.instance) {
            Singleton.instance = new Singleton();
        }
        return Singleton.instance;
    }
}

export default Singleton;
