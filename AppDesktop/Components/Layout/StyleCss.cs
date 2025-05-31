using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDesktop.Components.Layout;

public static class StyleCss
{
    public static string _styleCadastro => @" /* Mantenha todos os estilos anteriores */
            /*.register-cloud-container 
            {
                position: relative;
                width: 100%;
                min-height: 100vh;
                overflow: hidden;
                background: linear-gradient(135deg, #71b7e6, #9b59b6);
                display: flex;
                align-items: center;
                justify-content: center;
                padding: 16px;
            } */
            .register-cloud-container {
                position: relative;
                width: 100%;
                height: calc(100vh - 48px); /* Ocupa altura total menos a AppBar */
                overflow: hidden; /* Remove scroll interno */
                background: linear-gradient(135deg, #71b7e6, #9b59b6);
                display: flex;
                align-items: center;
                justify-content: center;
                padding: 16px;
            }
            .clouds {
                position: absolute;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                overflow: hidden;
                z-index: 0;
            }
            .cloud {
                position: absolute;
                background: rgba(255, 255, 255, 0.8);
                border-radius: 1000px;
                filter: blur(5px);
                opacity: 0.8;
            }
            .custom-card {
                position: relative;
                width: 100%;
                max-width: 450px;
                background: rgba(255, 255, 255, 0.1);
                backdrop-filter: blur(12px);
                border-radius: 12px;
                border: 2px solid rgba(255, 255, 255, 0.3);
                box-shadow: 0 8px 32px rgba(0, 0, 0, 0.2);
                z-index: 1;
                padding: 24px;
            }
            .card-content {
                color: #ffffff;
                text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.5);
            }
            .card-title {
                font-size: 1.75rem;
                font-weight: 600;
                margin-bottom: 1.5rem;
            }
            .custom-button {
                width: 100%;
                padding: 12px 24px;
                background-color: #8e44ad;
                color: #ffffff;
                border: none;
                border-radius: 4px;
                font-size: 1rem;
                font-weight: 500;
                cursor: pointer;
                transition: all 0.3s ease;
                text-shadow: 1px 1px 1px rgba(0, 0, 0, 0.4);
                box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
            }
            .custom-button:hover {
                background-color: #732d91;
                transform: translateY(-2px);
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.25);
            }

            .custom-link {
                color: #ecf0f1 !important;
                text-decoration: none;
                font-weight: 500;
                text-shadow: 1px 1px 1px rgba(0, 0, 0, 0.3);
            }

            .custom-link:hover {
                text-decoration: underline;
                color: #ffffff !important;
            }

            .custom-input-group .mud-input-control,
            .custom-input-group .mud-input-slot,
            .custom-input-group .mud-input-label {
                color: #ffffff !important;
                text-shadow: 1px 1px 1px rgba(0, 0, 0, 0.3);
            }

            /* Snackbar customization */
            .mud-snackbar-left {
                left: 20px;
                right: auto;
            }

            .mud-snackbar-content {
                background-color: #d32f2f !important;
                color: white !important;
            }

            .mud-snackbar-icon svg {
                color: white !important;
            }

            /* Responsividade */
            @media (max-width: 600px) {
                .register-cloud-container {
                    padding: 8px;
                }

                .custom-card {
                    padding: 16px;
                    border-radius: 8px;
                }

                .card-title {
                    font-size: 1.5rem;
                }

                .custom-button {
                    padding: 10px 20px;
                }}";

    public static string _styleLogin => @".login-cloud-container {
        position: relative;
        width: 100%;
        min-height: 100vh;
        overflow: hidden;
        background: linear-gradient(135deg, #71b7e6, #9b59b6);
        display: flex;
        align-items: center;
        justify-content: center;
        padding: 16px;
    }

    .clouds {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        overflow: hidden;
        z-index: 0;
    }

    .cloud {
        position: absolute;
        background: rgba(255, 255, 255, 0.8);
        border-radius: 1000px;
        filter: blur(5px);
        opacity: 0.8;
    }

        /* Tamanhos das nuvens responsivos */
        .cloud:nth-child(1) {
            width: clamp(200px, 30vw, 300px);
            height: clamp(60px, 10vw, 100px);
            top: 20%;
            left: -300px;
            animation: moveCloud 25s linear infinite;
        }

        .cloud:nth-child(2) {
            width: clamp(150px, 25vw, 200px);
            height: clamp(50px, 7vw, 70px);
            top: 40%;
            left: -200px;
            animation: moveCloud 20s linear infinite 5s;
        }

        .cloud:nth-child(3) {
            width: clamp(180px, 28vw, 250px);
            height: clamp(60px, 8vw, 80px);
            top: 60%;
            left: -250px;
            animation: moveCloud 30s linear infinite 10s;
        }

        .cloud:nth-child(4) {
            width: clamp(120px, 20vw, 180px);
            height: clamp(40px, 6vw, 60px);
            top: 30%;
            left: -180px;
            animation: moveCloud 22s linear infinite 7s;
        }

        .cloud:nth-child(5) {
            width: clamp(250px, 35vw, 350px);
            height: clamp(80px, 12vw, 120px);
            top: 70%;
            left: -350px;
            animation: moveCloud 35s linear infinite 15s;
        }

    @keyframes moveCloud {
        0%

    {
        transform: translateX(0);
    }

    100% {
        transform: translateX(calc(100vw + 300px));
    }

    }

    .login-box {
        position: relative;
        width: 100%;
        max-width: 400px;
        z-index: 1;
    }

    /* Ajustes para telas pequenas */
    @media (max-width: 600px) {
        .login-cloud-container {
            padding: 8px;
        }

        .MudCard {
            border-radius: 8px !important;
        }

        .MudTextField {
            margin-bottom: 12px !important;
        }

        .MudButton {
            height: 48px !important;
        }
    } */´";

}

public interface IStyleCss
{
    string GetStyle(string style);
}
