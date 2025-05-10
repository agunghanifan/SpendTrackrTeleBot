CREATE TABLE IF NOT EXISTS users (
    ID SERIAL PRIMARY KEY,
    CHAT_ID VARCHAR(100) NOT NULL,
    SHEET_ID VARCHAR(100) NOT NULL,
    EMAIL VARCHAR(255) NOT NULL,
    STATUS_DATA VARCHAR(50) DEFAULT 'Y',
    CREATED_DATE TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UPDATED_DATE TIMESTAMP NOT NULL
);

-- Create indexes for better query performance
CREATE INDEX IF NOT EXISTS idx_users_chat_id ON users(CHAT_ID);
CREATE INDEX IF NOT EXISTS idx_users_sheet_id ON users(SHEET_ID);
CREATE INDEX IF NOT EXISTS idx_users_email ON users(EMAIL);
CREATE INDEX IF NOT EXISTS idx_users_status ON users(STATUS_DATA);
CREATE INDEX IF NOT EXISTS idx_users_created_date ON users(CREATED_DATE);
CREATE INDEX IF NOT EXISTS idx_users_updated_date ON users(UPDATED_DATE);

-- Create a composite index for common query patterns
CREATE INDEX IF NOT EXISTS idx_users_chat_sheet ON users(CHAT_ID, SHEET_ID);