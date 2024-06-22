import json
import requests


img_upscale = '/v1/generation/image-upscale-vary'
inpaint_outpaint = '/v1/generation/image-inpaint-outpaint'
img_prompt = '/v1/generation/image-prompt'

def text2image(params: dict) -> dict:
    fooocus_host = 'http://127.0.0.1:8888'
    text2image = '/v1/generation/text-to-image'

    data = json.dumps(params)
    response = requests.post(
        url=f"{fooocus_host}{text2image}",
        data=data,
        timeout=300)
    return response.json()


t2i_params = {
    "prompt": "a dog",
    "performance_selection": "Lightning",
    "aspect_ratios_selection": "896*1152",
    "async_process": False
}

t2i_result = text2image(params=t2i_params)
print(json.dumps(t2i_result))
