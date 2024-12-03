﻿namespace ProcraftAPI.Services
{
    public class EmailTemplateService
    {
        public async Task<string> GetHtmlTemplate()
        {
            return
                """"
                                <!DOCTYPE html>
                <html lang="en">
                	<head>
                		<meta charset="UTF-8">
                		<meta name="viewport"
                			content="width=device-width, initial-scale=1.0">
                		<title>Email de recuperação</title>

                <style>
                * {
                    padding: 0;
                    margin: 0;
                }

                body {
                    position: relative;
                    height: 100vh;
                    width: 100vw;
                    min-width: 720px;
                    font-family: Arial, Helvetica, sans-serif;
                    color: #391D37;

                    .ilustration {
                        img {
                            position: absolute;
                            border-radius: 100%;
                            height: 24vw;
                            min-height: 240px;
                            width: 24vw;
                            min-width: 240px;
                            z-index: 1;
                            right: 0;
                        }
                    }

                    .header-background {
                        position: relative;
                        background-color: #7C2FB8;
                        width: 100%;
                        height: 24%;

                        .email-header {
                            background-color: #391D37;
                            width: 100%;
                            height: 80%;
                        }
                    }

                    .footer-background {
                        display: flex;
                        flex-direction: column;
                        justify-content: start;
                        background-color: #391D37;
                        width: 100%;
                        height: 24%;

                        .email-footer {
                            background-color: #7C2FB8;
                            width: 100%;
                            height: 20%;
                        }
                    }

                    .container {
                        position: relative;
                        background-color: white;
                        border-radius: 36px;
                        width: 60%;
                        min-width: 540px;
                        margin: 0 auto;
                        display: flex;
                        flex-direction: column;
                        justify-content: center;
                        align-items: center;
                        top: -12%;

                        h1 {
                            padding-bottom: 24px;
                        }

                        p {
                            padding-bottom: 24px;
                        }

                        h3 {
                            font-size: 48px;
                        }
                    }
                }
                </style>

                	</head>
                	<body>
                		<div class="ilustration">
                			<img src="./assets/email_template_img.jpg" alt="woman_holding_tablet">
                		</div>
                		<div class="header-background">
                			<div class="email-header"></div>
                		</div>
                		<main class="container">
                			<img src="./assets/procraft_logo.png" alt="procraft_logo" width="180">
                			<h1>Procraft</h1>
                			<p>Gerência de procedimentos</p>
                			<div class="content"></div>
                				<h2>
                					Olá, {Usuário}!
                				</h2>
                				<br>
                				<p>
                					Utilize o código a seguir para recuperar sua conta no app Procraft
                				</p>
                				<h3>
                					BD3359
                				</h3>
                			</div>
                		</main>
                		<div class="footer-background">
                			<div class="email-footer"></div>
                		</div>
                	</body>
                </html>
                """";
        }
    }
}