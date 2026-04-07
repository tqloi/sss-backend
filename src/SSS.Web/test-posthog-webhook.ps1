# ============================================================
# Test PostHog Webhook API
# Usage: .\test-posthog-webhook.ps1
# ============================================================

# --- Cấu hình ---
$baseUrl = "https://localhost:7243"       # <-- đổi port cho đúng
$secret  = "THIS_IS_A_SUPER_LONG_RANDOM_SECRET_AT_LEAST_32_CHARS"

# --- Body giả lập PostHog event ---
$body = @{
    event       = "page_view"
    distinct_id = "user_12345"
    timestamp   = "2026-03-26T19:00:00Z"
    properties  = @{
        url      = "https://app.studysense.com/dashboard"
        referrer = "https://google.com"
        browser  = "Chrome"
    }
} | ConvertTo-Json -Depth 10

# --- Signature = raw secret (PostHog gửi static header) ---
$signature = $secret

Write-Host "=== PostHog Webhook Test ===" -ForegroundColor Cyan
Write-Host "URL:       $baseUrl/api/posthog/webhook"
Write-Host "Signature: [using raw secret]"
Write-Host ""

# --- Test 1: Gửi request hợp lệ ---
Write-Host "[TEST 1] Valid signature..." -ForegroundColor Green
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/posthog/webhook" `
        -Method POST `
        -ContentType "application/json" `
        -Headers @{ "X-PostHog-Signature" = $signature } `
        -Body $body
    Write-Host "SUCCESS: $($response | ConvertTo-Json)" -ForegroundColor Green
} catch {
    Write-Host "FAILED: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# --- Test 2: Signature sai → expect 401 ---
Write-Host "[TEST 2] Invalid signature (expect 401)..." -ForegroundColor Yellow
try {
    Invoke-RestMethod -Uri "$baseUrl/api/posthog/webhook" `
        -Method POST `
        -ContentType "application/json" `
        -Headers @{ "X-PostHog-Signature" = "invalid" } `
        -Body '{"event":"test"}'
    Write-Host "UNEXPECTED SUCCESS (should have been 401)" -ForegroundColor Red
} catch {
    $status = $_.Exception.Response.StatusCode.value__
    if ($status -eq 401) {
        Write-Host "CORRECT: Got 401 Unauthorized" -ForegroundColor Green
    } else {
        Write-Host "UNEXPECTED STATUS: $status" -ForegroundColor Red
    }
}
