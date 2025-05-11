const API_GATEWAY_URL = "http://localhost:5000/api";

// Ahora dejamos tus variables originales usando el Gateway
const API_BASE_URL = API_GATEWAY_URL;
export const AUTH_API = API_BASE_URL;
export const ARTICLES_API = API_GATEWAY_URL;
export const SUGGEST_API = API_GATEWAY_URL;

export default API_BASE_URL;