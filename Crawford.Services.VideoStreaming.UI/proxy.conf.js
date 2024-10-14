const PROXY_HOST = "https://localhost:7220/";

const PROXY_CONFIG = [
    {
        context: ['/api'],
        target: PROXY_HOST,
        secure: false
    }
];

module.exports = PROXY_CONFIG;