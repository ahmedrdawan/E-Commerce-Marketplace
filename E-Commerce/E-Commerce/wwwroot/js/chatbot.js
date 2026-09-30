// chatbot.js
document.addEventListener('DOMContentLoaded', () => {
    const toggleBtn = document.getElementById('chatbotToggleBtn');
    const panel = document.getElementById('chatbotPanel');
    const closeBtn = document.getElementById('chatbotCloseBtn');
    const messageContainer = document.getElementById('chatbotMessages');
    const inputField = document.getElementById('chatbotInput');
    const sendBtn = document.getElementById('chatbotSendBtn');
    const typingIndicator = document.getElementById('chatbotTypingIndicator');

    let hasOpened = false;

    // Retrieve the anti-forgery token rendered in the form
    const tokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
    const token = tokenElement ? tokenElement.value : '';

    toggleBtn.addEventListener('click', () => {
        panel.classList.toggle('chatbot-open');
        if (panel.classList.contains('chatbot-open') && !hasOpened) {
            hasOpened = true;
            appendMessage("Hi! I'm your virtual shopping assistant. How can I help you today?", 'bot');
        }
    });

    closeBtn.addEventListener('click', () => {
        panel.classList.remove('chatbot-open');
    });

    inputField.addEventListener('keypress', (e) => {
        if (e.key === 'Enter') {
            e.preventDefault();
            sendMessage();
        }
    });

    sendBtn.addEventListener('click', sendMessage);

    async function sendMessage() {
        const message = inputField.value.trim();
        if (!message) return;

        appendMessage(message, 'user');
        inputField.value = '';
        inputField.disabled = true;
        sendBtn.disabled = true;
        typingIndicator.style.display = 'block';
        scrollToBottom();

        try {
            const response = await fetch('/Chat/SendMessage', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify({ message })
            });

            const data = await response.json();

            if (!response.ok) {
                appendMessage(data || "An error occurred.", 'error');
            } else {
                appendMessage(data.reply, 'bot');
            }
        } catch (error) {
            appendMessage("Unable to connect to the server.", 'error');
        } finally {
            typingIndicator.style.display = 'none';
            inputField.disabled = false;
            sendBtn.disabled = false;
            inputField.focus();
            scrollToBottom();
        }
    }

    function appendMessage(text, sender) {
        const messageDiv = document.createElement('div');
        messageDiv.classList.add('chatbot-message', sender);
        messageDiv.textContent = text;
        messageContainer.insertBefore(messageDiv, typingIndicator);
        scrollToBottom();
    }

    function scrollToBottom() {
        messageContainer.scrollTop = messageContainer.scrollHeight;
    }
});