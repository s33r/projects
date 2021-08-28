
const defaultOptions = {
    host: 'host',
    wi

};


class Global {


    constructor(host) {
        this.host = document.getElementById(host);
        this.canvas = document.createElement('canvas');
        this.context = this.canvas.getContext("2d");

        this.canvas.setAttribute('width', window.innerWidth);
        this.canvas.setAttribute('height', window.innerHeight);

        this.host.appendChild(this.canvas);
    }

    start() {


        
        this.context.moveTo(0, 0);
        this.context.lineTo(200, 100);
        this.context.stroke();
    }
}

let singleton;


export const initialize = function initialize(options) {
    const actualOptions = {
        ...defaultOptions,
        ...options,
    };

    console.log(actualOptions);

    singleton = new Global(actualOptions.host);

    singleton.start();
}

export default singleton;